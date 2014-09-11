namespace LicenseeAgent.Authentication.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Fanex.Common;
    using LicenseeAgent.Authentication.Criteria.Agent;
    using LicenseeAgent.Authentication.Services;
    using LicenseeAgent.Authentication.ViewModels.Agent;
    using LicenseeAgent.Authentication.ViewResults.Agent;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// Agent controller.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    public class AgentController : BaseController
    {
        #region Variables

        private readonly IAgentService _accountService;
        private readonly ICoreService _coreService;
        private readonly IAuthenticateService _authenticateService;

        #endregion Variables

        #region Constructors

        public AgentController()
            : this(null, null, null)
        {
        }

        public AgentController(
            IAgentService accountService,
            ICoreService coreService,
            IAuthenticateService authenticateService)
        {
            _accountService = accountService ?? new AgentService();
            _coreService = coreService ?? new CoreService();
            _authenticateService = authenticateService ?? new AuthenticateService();
        }

        #endregion Constructors

        #region Authentication

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
                return RedirectToAction("SignIn", "Admin", new { area = "Authentication" });
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
    }
}