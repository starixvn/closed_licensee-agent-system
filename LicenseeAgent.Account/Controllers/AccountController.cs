namespace LicenseeAgent.Account.Controllers
{
    using System;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Fanex.Common;
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.Account.Criteria.Account;
    using LicenseeAgent.Account.Criteria.Internal;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Account.ViewModels;
    using LicenseeAgent.Account.ViewResults;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Core;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// Account controllers.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    public class AccountController : BaseController
    {
        #region Variables

        private readonly IAccountService _accountService;
        private readonly IInternalService _internalService;
        private readonly ICoreService _coreService;
        private readonly ICaptchaService _captchaService;
        private readonly IAuthenticateService _authenticateService;

        #endregion Variables

        #region Constructors

        public AccountController()
            : this(null, null, null, null, null)
        {
        }

        public AccountController(
            IAccountService accountService,
            IInternalService internalService,
            ICoreService coreService,
            ICaptchaService captchaService,
            IAuthenticateService authenticateService)
        {
            _accountService = accountService ?? new AccountService();
            _internalService = internalService ?? new InternalService();
            _coreService = coreService ?? new CoreService();
            _captchaService = captchaService ?? new CaptchaService();
            _authenticateService = authenticateService ?? new AuthenticateService();
        }

        #endregion Constructors

        #region Authentication

        /// <summary>
        /// Generate Captcha Image.
        /// </summary>
        /// <returns>
        /// Return image.
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult CaptchaImage()
        {
            var captcha = _captchaService.Generate();
            using (var memoryStream = new MemoryStream())
            {
                Session[Constants.AppSessionCaptcha] = captcha.Text;
                captcha.Image.Save(memoryStream, ImageFormat.Png);
                return File(memoryStream.GetBuffer(), "image/png");
            }
        }

        /// <summary>
        /// Sign-in page.
        /// </summary>
        /// <returns>Returns Sign-in HTML content.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            if (AppSettings.IsAdminSite)
            {
                return RedirectToAction("SignIn", "Internal");
            }

            var currentLanguage = Language;
            var captcha = AppSettings.IsLocalRequest ? Constants.LocalTestCaptcha : string.Empty;
            var model = new AgentSignInViewModel
            {
                Language = currentLanguage,
                Captcha = captcha
            };
            return View(model);
        }

        /// <summary>
        /// Post SignIn Page.
        /// </summary>
        /// <param name="model">Input model.</param>
        /// <returns>Redirect to home page.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(AgentSignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var captcha = Session[Constants.AppSessionCaptcha] as string;
                if (string.IsNullOrEmpty(captcha))
                {
                    ModelState.AddModelError("Captcha", Las.SessionError);
                }
                else
                {
                    if ((AppSettings.IsLocalRequest && model.Captcha == Constants.LocalTestCaptcha)
                        || string.Compare(captcha, model.Captcha, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        var licenseeId = _coreService.GetLicenseeId(Request);
                        if (licenseeId == null)
                        {
                            ModelState.AddModelError(string.Empty, string.Format(Las.NoLicenseeDomainFound, Request.Url.Host));
                        }
                        else if (AuthenticateUser(model, licenseeId.Value))
                        {
                            return CheckAgentPasswordStatus(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, Las.IncorrectCaptchaMsg);
                    }
                }
            }

            // Always remove captcha value from input because it can't be reused.
            if (!ModelState.IsValid && !AppSettings.IsLocalRequest)
            {
                ModelState.Remove("Captcha");
                model.Captcha = string.Empty;
            }

            return View(model);
        }

        /// <summary>
        /// Validates the Agent user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="licenseeId">Name of the domain.</param>
        /// <returns>
        /// Returns true whether username/password is valid.
        /// </returns>
        [NonAction]
        public bool AuthenticateUser(AgentSignInViewModel model, int licenseeId)
        {
            var userSaltCriteria = new AgentSaltSelectCriteria
            {
                UserName = model.Username,
                LicenseeId = licenseeId
            };

            var userSalt = _accountService.GetAgentSalt(userSaltCriteria);
            if (userSaltCriteria.ErrCode != Result.SuccessValue)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(userSaltCriteria.ErrCode));
                return false;
            }

            var clientInfo = Request.GetClientInfo();
            var criteria = new AgentLoginCriteria
            {
                UserName = model.Username,
                UserPwd = Cryptography.CreateHash(model.Password, userSalt),
                UserSalt = userSalt,
                LoginIp = clientInfo.Ip,
                LicenseeId = licenseeId,
                BrowserName = clientInfo.BrowserName,
                Platform = clientInfo.Platform,
                CountryCode = clientInfo.CountryCode
            };

            var agentInfo = _accountService.AuthenticateAgent(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                var errorMessage = GetErrMsg(criteria.ErrCode);
                errorMessage = string.Format(errorMessage, criteria.LastChangedPwdDate.ToShortDateString());
                ModelState.AddModelError(string.Empty, errorMessage);
                return false;
            }

            CheckIn(agentInfo, model.Language);
            return true;
        }

        /// <summary>
        /// Checks the agent password status.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns view.</returns>
        [NonAction]
        public ActionResult CheckAgentPasswordStatus(AgentSignInViewModel model)
        {
            // check account password
            var agentInfo = CurrentSession.AgentInfo;
            var passwordStatus = PasswordRule.GetPasswordStatus(agentInfo, agentInfo.LastLoggedTime);
            if (passwordStatus == PasswordStatus.Normal)
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }

            ViewBag.Result = new SignInViewResult
            {
                IsConfirm = passwordStatus == PasswordStatus.Alert,
                Message = PasswordRule.GetStatusMessage(passwordStatus, agentInfo.PwdExpiry)
            };

            return View(model);
        }

        /// <summary>
        /// Store current user session.
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <param name="language">The language.</param>
        [NonAction]
        public void CheckIn(AgentInfo agentInfo, string language)
        {
            var criteria = new AgentCheckInCriteria
            {
                AgentId = agentInfo.AgentId,
                Ip = Request.GetClientIp(),
                NetSessionId = Session.SessionID
            };

            _accountService.AgentCheckIn(criteria);
            SessionManager.CurrentSession = new CurrentSession(agentInfo, criteria.OnUserId, language);
            _authenticateService.Login(agentInfo.UserName);
        }

        #endregion Authentication

        #region Change password

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <returns>Returns view.</returns>
        [HttpGet]
        [AppAuthorize(IsValidateAgent = false)]
        [AccountType(AccountType.Agent)]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns a result.</returns>
        [HttpPost]
        [AppAuthorize(IsValidateAgent = false)]
        [AccountType(AccountType.Agent)]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(AgentChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldPassword = Cryptography.CreateHash(model.CurrentPassword, CurrentSession.AgentInfo.UserSalt);
                var newPassword = Cryptography.CreateHash(model.NewPassword, CurrentSession.AgentInfo.UserSalt);
                var criteria = new AgentChangePasswordCriteria
                {
                    Updater = CurrentSession.LoginUserId,
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    OldPassword = oldPassword,
                    Password = newPassword,
                    LoginIp = Request.GetClientIp()
                };

                _accountService.AgentChangePassword(criteria);
                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ViewBag.Result = new AgentChangePasswordViewResult
                    {
                        Message = Las.UpdateSuccessfully,
                        IsReload = CurrentSession.IsAgentChangePassword()
                    };

                    // after changing password successfully, re-update some status to not validate user password again.
                    CurrentSession.AgentInfo.IsFirstLogin = false;
                    CurrentSession.AgentInfo.PwdExpiry = DateTime.Today.AddDays(PasswordRule.PasswordExpiredTotalDays);
                }
            }

            return View();
        }

        #endregion Change password

        #region ReCheckIn

        /// <summary>
        /// Post ReCheckIn.
        /// </summary>
        /// <returns>
        /// Return true or false.
        /// </returns>
        [HttpPost]
        [AjaxOnly]
        [AppAuthorize(IsValidateAgent = false)]
        public JsonResult ReCheckIn()
        {
            var errCode = Result.FailureValue;
            if (CurrentSession.LoginAccountType == AccountType.CSDIT
                || CurrentSession.LoginAccountType == AccountType.Licensee)
            {
                errCode = AdminReCheckIn();
            }
            else if (CurrentSession.LoginAccountType == AccountType.Agent)
            {
                errCode = AgentReCheckIn();
            }

            switch (errCode)
            {
                case (int)ErrCode.Msg_1037:
                    ClearSession(true);
                    break;

                case (int)ErrCode.Msg_1038:
                    ClearSession(false);
                    break;
            }

            return JsonResult(errCode);
        }

        /// <summary>
        /// Admin re-check in on current session to kick out when having another login session behalf on current admin.
        /// </summary>
        /// <returns>
        /// Returns true whether having another login session.
        /// </returns>
        [NonAction]
        public int AdminReCheckIn()
        {
            var criteria = new AdminReCheckInCriteria
            {
                OnAdminId = CurrentSession.OnUserId,
                OnLicenseeId = CurrentSession.OnLicenseeId == 0 ? null : (int?)CurrentSession.OnLicenseeId
            };

            _internalService.AdminReCheckIn(criteria);
            return criteria.ErrCode;
        }

        /// <summary>
        /// Agent re-check in on current session to kick out when having another login session behalf on current agent.
        /// </summary>
        /// <returns>Returns true whether having another login session.</returns>
        [NonAction]
        public int AgentReCheckIn()
        {
            var criteria = new AgentReCheckInCriteria
            {
                OnAgentId = CurrentSession.OnUserId
            };

            _accountService.AgentReCheckIn(criteria);
            return criteria.ErrCode;
        }

        #endregion ReCheckIn

        #region SignOut

        /// <summary>
        /// Sign out action.
        /// </summary>
        /// <returns>Redirect to Login Page.</returns>
        [AppAuthorize(IsValidateAgent = false)]
        public ActionResult SignOut()
        {
            ClearSession(true);
            if (HttpContext.Response.IsRequestBeingRedirected)
            {
                return null;
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// Clears the session.
        /// </summary>
        /// <param name="clearCheckin">If set to <c>true</c> [clear check-in].</param>
        [NonAction]
        public void ClearSession(bool clearCheckin)
        {
            if (clearCheckin)
            {
                if (CurrentSession.IsAdmin)
                {
                    AdminUserLogout();
                }
                else
                {
                    AgentUserLogout();
                }
            }

            _authenticateService.Logout();
            SessionManager.ClearAll();
        }

        /// <summary>
        /// Remove Agent Session.
        /// </summary>
        [NonAction]
        public void AgentUserLogout()
        {
            var criteria = new AgentUserLogoutCriteria
            {
                OnAgentId = CurrentSession.OnUserId
            };

            _accountService.AgentUserLogout(criteria);
        }

        /// <summary>
        /// Remove Admin Session.
        /// </summary>
        [NonAction]
        public void AdminUserLogout()
        {
            var criteria = new AdminUserLogoutCriteria
            {
                OnAdminId = CurrentSession.OnUserId
            };

            _internalService.AdminUserLogout(criteria);
        }

        #endregion SignOut

        #region Change language

        /// <summary>
        /// Call to change language.
        /// </summary>
        /// <param name="language">The selected language.</param>
        /// <returns>Returns action status.</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangeLanguage(string language)
        {
            var result = false;
            if (Language != language)
            {
                Language = language;
                result = true;
            }

            return Json(new
            {
                isChanged = result
            });
        }

        #endregion Change language

        #region Avatar

        [HttpGet]
        [AppAuthorize(IsValidateAgent = false)]
        public ActionResult Avatar()
        {
            var avatarImage = Path.Combine(SitePath.GlobalImagePath, "avatar.png");
            var base64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(avatarImage));
            var imgSrc = string.Format("data:image/gif;base64,{0}", base64);
            return Content(imgSrc);
        }

        #endregion Avatar
    }
}