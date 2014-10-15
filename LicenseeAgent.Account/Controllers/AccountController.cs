namespace LicenseeAgent.Account.Controllers
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Fanex.Common;
    using LicenseeAgent.Account.Criteria.Account;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Account.ViewModels.Account;
    using LicenseeAgent.Account.ViewResults.Account;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Core;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// AccountController class.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    public class AccountController : BaseController
    {
        #region Variables

        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authenticateService;

        #endregion Variables

        #region Constructors

        public AccountController()
            : this(null, null)
        {
        }

        public AccountController(
            IAccountService accountService,
            IAuthenticateService authenticateService)
        {
            _accountService = accountService ?? new AccountService();
            _authenticateService = authenticateService ?? new AuthenticateService();
        }

        #endregion Constructors

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

                    // after changing password successfully, re-update status to not validate user password again.
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

            _accountService.AdminReCheckIn(criteria);
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

            _accountService.AdminUserLogout(criteria);
        }

        #endregion SignOut

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
            if (Language != language)
            {
                Language = language;
            }

            return JsonResult(new Result(Result.SuccessValue));
        }

        #endregion Change language
    }
}