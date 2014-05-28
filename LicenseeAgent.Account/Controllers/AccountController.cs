// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Fanex.Common;
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.Account.Entities;
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

    /// <summary>
    /// Account controllers.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        private readonly ICoreService _coreService;

        public AccountController(IAccountService accountService, ICoreService coreService)
        {
            _accountService = accountService;
            _coreService = coreService;
        }

        /// <summary>
        /// Home page.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new HomePageViewModel
            {
                StatisticInfo = new StatisticEntity(),
                BalanceInfo = new BalanceInfoEntity()
            };

            if (CurrentSession.AgentInfo != null)
            {
                var statisticCriteria = new StatisticCriteria { AgentID = CurrentSession.AgentInfo.AgentId };
                var statisticInfo = _accountService.GetStatisticInfo(statisticCriteria).FirstOrDefault();

                var balanceUp = _coreService.GetBalanceUpDay();
                var balanceInfoCriteria = new BalanceInfoCriteria
                {
                    AgentID = CurrentSession.AgentInfo.AgentId,
                    MoveBetDay = balanceUp.MoveBetsDay
                };
                var balanceInfo = _accountService.GetBalanceInfo(balanceInfoCriteria).FirstOrDefault();

                model = new HomePageViewModel
                {
                    StatisticInfo = statisticInfo,
                    BalanceInfo = balanceInfo
                };
            }

            return View(model);
        }

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
            var captchaImage = new CaptchaImage
            {
                Width = 119,
                Height = 32
            };

            using (var mem = new MemoryStream())
            {
                var img = captchaImage.RenderImage();
                Session[Constants.AppSessionCaptcha] = captchaImage.Text;
                img.Save(mem, System.Drawing.Imaging.ImageFormat.Png);

                return File(mem.GetBuffer(), "image/png");
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
            var model = new AgentSignInViewModel
            {
                Language = currentLanguage,
                Captcha = AppSettings.LocalTest ? "999999" : string.Empty
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
                    if ((AppSettings.LocalTest && model.Captcha == "999999") || string.Compare(captcha, model.Captcha, true) == 0)
                    {
                        var licenseeId = DomainMapper.GetLicenseeId(Request);
                        if (licenseeId == null)
                        {
                            ModelState.AddModelError(string.Empty, string.Format(Las.NoLicenseeDomainFound, Request.Url.Host));
                        }
                        else
                        {
                            if (AuthenticateUser(model, licenseeId.Value))
                            {
                                return CheckAgentPasswordStatus(model);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, Las.IncorrectCaptchaMsg);
                    }
                }
            }

            // Always remove captcha value from input because it can't be reused.
            if (!ModelState.IsValid && !AppSettings.LocalTest)
            {
                ModelState.Remove("Captcha");
                model.Captcha = string.Empty;
            }

            return View(model);
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
                var criteria = new AgentChangePwdCriteria
                {
                    Updater = CurrentSession.LoginUserId,
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    OldPwd = oldPassword,
                    Pwd = newPassword,
                    LoginIp = Request.GetClientIp()
                };

                _accountService.AgentChangePassword(criteria);
                if (criteria.MsgId != 0)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.MsgId));
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
            if (CurrentSession.LoginAccountType == AccountType.CSD_IT
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

        [NonAction]
        public int AgentReCheckInLicensee()
        {
            var criteria = new AgentReCheckInLicenseeCriteria
            {
                OnAdminId = CurrentSession.OnUserId,
                LicenseeId = CurrentSession.OnLicenseeId
            };

            _accountService.AgentReCheckInLicensee(criteria);
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

            FormsAuthentication.SignOut();
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

        [HttpGet]
        [AppAuthorize(IsValidateAgent = false)]
        public ActionResult Avatar()
        {
            var avatarImage = Path.Combine(SitePath.GlobalImagePath, "avatar.png");
            var base64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(avatarImage));
            var imgSrc = string.Format("data:image/gif;base64,{0}", base64);
            return Content(imgSrc);
        }

        #region Statistic Drilldown

        /// <summary>
        /// Statistics top 10 winning members.
        /// </summary>
        /// <returns>View result.</returns>
        public ViewResult TopWinning()
        {
            var balanceUp = _coreService.GetBalanceUpDay();
            var criteria = new TopWinningCriteria
            {
                AgentID = CurrentSession.AgentInfo.AgentId,
                BalanceUpDay = balanceUp.BalanceUpDay,
                MoveBetsDay = balanceUp.MoveBetsDay
            };
            var topWinningData = _accountService.GetTopWinning(criteria);

            return View(topWinningData);
        }

        /// <summary>
        /// Statistics top 10 turnover members.
        /// </summary>
        /// <returns>View result.</returns>
        public ViewResult TopTurnover()
        {
            var balanceUp = _coreService.GetBalanceUpDay();
            var criteria = new TopTurnoverCriteria
            {
                AgentID = CurrentSession.AgentInfo.AgentId,
                BalanceUpDay = balanceUp.BalanceUpDay,
                MoveBetsDay = balanceUp.MoveBetsDay
            };
            var topTurnoverData = _accountService.GetTopTurnover(criteria);

            return View(topTurnoverData);
        }

        /// <summary>
        /// New members of the month.
        /// </summary>
        /// <returns>View result.</returns>
        public ViewResult NewMembers()
        {
            var criteria = new NewMemberOfMonthCriteria { AgentID = CurrentSession.AgentInfo.AgentId };
            var newMemberData = _accountService.GetNewMemberOfMonth(criteria);

            return View(newMemberData);
        }

        #endregion Statistic Drilldown

        /// <summary>
        /// Validates the Agent user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="licenseeId">Name of the domain.</param>
        /// <returns>
        /// Returns true whether username/password is valid.
        /// </returns>
        private bool AuthenticateUser(AgentSignInViewModel model, int licenseeId)
        {
            var userSaltCriteria = new AgentSaltSelCriteria
            {
                UserName = model.Username,
                LicenseeId = licenseeId
            };

            var userSalt = _accountService.GetAgentSalt(userSaltCriteria);
            if (userSaltCriteria.ErrCode != 0)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(userSaltCriteria.ErrCode));
                return false;
            }

            var clientInfo = Request.GetClientInfo();
            var criteria = new AgentLoginCriteria
            {
                UserName = model.Username,
                UserPwd = model.Password,
                UserSalt = userSalt,
                LoginIp = clientInfo.Ip,
                LicenseeId = licenseeId,
                BrowserName = clientInfo.BrowserName,
                Platform = clientInfo.Platform,
                CountryCode = clientInfo.CountryCode
            };

            var agentInfo = _accountService.AuthenticateAgent(criteria);
            if (criteria.ErrCode != 0)
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
        /// Store current user session.
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <param name="language">The language.</param>
        private void CheckIn(AgentInfo agentInfo, string language)
        {
            var criteria = new AgentCheckInCriteria
            {
                AgentId = agentInfo.AgentId,
                Ip = Request.GetClientIp(),
                NetSessionId = Session.SessionID
            };

            _accountService.AgentCheckIn(criteria);
            SessionManager.CurrentSession = new CurrentSession(agentInfo, criteria.OnUserId, language);
            FormsAuthentication.SetAuthCookie(agentInfo.UserName, true);
        }
    }
}