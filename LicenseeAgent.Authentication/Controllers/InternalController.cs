namespace LicenseeAgent.Authentication.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using LicenseeAgent.API.LRF;
    using LicenseeAgent.Authentication.Criteria;
    using LicenseeAgent.Authentication.Criteria.Internal;
    using LicenseeAgent.Authentication.Services;
    using LicenseeAgent.Authentication.ViewModels.Internal;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// Internal Login Controller.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    public class InternalController : BaseController
    {
        #region Variables

        private readonly IInternalService _internalService;
        private readonly ICoreService _coreService;
        private readonly Integration _integration;

        #endregion Variables

        #region Constructors

        public InternalController()
            : this(null, null, null)
        {
        }

        public InternalController(
            IInternalService internalService,
            ICoreService coreService,
            Integration integration)
        {
            _internalService = internalService ?? new InternalService();
            _coreService = coreService ?? new CoreService();
            _integration = integration ?? new Integration();
        }

        #endregion Constructors

        #region Internal login from LRF

        /// <summary>
        /// Internal login from LRF.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>Returns a page after login.</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Lrf(string authKey)
        {
            var internalInfo = _integration.GetInternalInfo(authKey);
            if (internalInfo == null)
            {
                return Content(Las.SessionExpired);
            }

            if (internalInfo.IsInternal == AppSettings.IsAdminSite)
            {
                return Content("Stop hacking");
            }

            var clientInfo = Request.GetClientInfo();
            var criteria = new LicenseeAdminLoginCriteria
            {
                LicenseeId = internalInfo.LicenseeId,
                UserId = internalInfo.UserId,
                LoginIP = clientInfo.Ip,
                BrowserName = clientInfo.BrowserName,
                Platform = clientInfo.Platform,
                CountryCode = clientInfo.CountryCode
            };
            _internalService.LicenseeLogin(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                return Content(GetErrMsg(criteria.ErrCode));
            }

            var adminInfo = new AdminInfo
            {
                UserId = criteria.UserId,
                UserName = criteria.UserName,
                AccountType = AppSettings.IsAdminSite ? AccountType.CSDIT : AccountType.Licensee
            };

            LicenseeCheckIn(adminInfo, criteria.LicenseeId, internalInfo.Language);
            return Redirect(FormsAuthentication.DefaultUrl);
        }

        /// <summary>
        /// CSD/IT or Licensee Admin check-in when internal log-in from LRF.
        /// </summary>
        /// <param name="adminInfo">The admin information.</param>
        /// <param name="licenseeId">The licensee identifier.</param>
        /// <param name="language">The language.</param>
        [NonAction]
        public void LicenseeCheckIn(AdminInfo adminInfo, int licenseeId, string language)
        {
            var criteria = new AdminCheckInCriteria
            {
                AdminId = adminInfo.UserId,
                IP = Request.GetClientIp(),
                NetSessionId = Session.SessionID,
                LicenseeId = licenseeId
            };

            _internalService.LicenseeCheckIn(criteria);
            var currentSession = new CurrentSession(adminInfo, criteria.OnAdminId, licenseeId, language);
            SessionManager.CurrentSession = currentSession;
            FormsAuthentication.SetAuthCookie(adminInfo.UserName, true);
        }

        #endregion Internal login from LRF

        #region Internal login as agent

        /// <summary>
        /// Internal login as agent.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>
        /// JSON null when update user role.
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoginAsUser(string authKey)
        {
            if (string.IsNullOrEmpty(authKey))
            {
                return Content(Las.TicketNotFound);
            }

            var internalLoginInfo = _internalService.Decode(authKey);
            if (internalLoginInfo == null || internalLoginInfo.IsExpired)
            {
                return Content(Las.TicketInvalidExpired);
            }

            var agentInfo = _internalService.InternalGetAgentInfo(new InternalGetAgentInfoCriteria
            {
                LicenseeId = internalLoginInfo.LicenseeId,
                CurrencyId = internalLoginInfo.CurrencyId,
                UserName = internalLoginInfo.AgentName
            });
            SessionManager.CurrentSession = new CurrentSession(agentInfo, internalLoginInfo);
            FormsAuthentication.SetAuthCookie(agentInfo.UserName, true);
            return Redirect(FormsAuthentication.DefaultUrl);
        }

        /// <summary>
        /// Checks the user is valid before make a internal login.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON.</returns>
        [HttpPost]
        [AppAuthorize(IsValidateAgent = false)]
        [AccountType(AccountType.Admin)]
        public JsonResult LoginAsUser(LoginAsUserViewModel model)
        {
            var clientInfo = Request.GetClientInfo();
            var criteria = new InternalLoginAsUserCriteria
            {
                AdminId = CurrentSession.LoginUserId,
                LicenseeId = model.LicenseeId,
                CurrencyId = model.CurrencyId,
                UserName = model.UserName,
                LoginIp = clientInfo.Ip,
                BrowserName = clientInfo.BrowserName,
                Platform = clientInfo.Platform,
                CountryCode = clientInfo.CountryCode
            };

            _internalService.InternalLoginAsAgent(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                return JsonResult(criteria.ErrCode);
            }

            var internalLoginInfo = new InternalLoginInfo
            {
                LoginUserId = CurrentSession.LoginUserId,
                LoginAccountType = CurrentSession.LoginAccountType,
                LoginUserName = CurrentSession.LoginUserName,
                OnLoginUserId = CurrentSession.OnUserId,
                Language = CurrentSession.Language,
                LicenseeId = criteria.LicenseeId,
                CurrencyId = criteria.CurrencyId,
                AgentName = model.UserName,
                IsInternalLRF = CurrentSession.IsInternalLRF
            };

            return string.IsNullOrEmpty(internalLoginInfo.AgentName)
                ? LoginAsAgent0(internalLoginInfo)
                : LoginAsAgentk(internalLoginInfo);
        }

        /// <summary>
        /// Admins internal login as agent.
        /// </summary>
        /// <param name="internalLoginInfo">The admin internal login information.</param>
        /// <returns>
        /// Returns JSON result.
        /// </returns>
        [NonAction]
        public JsonResult LoginAsAgent0(InternalLoginInfo internalLoginInfo)
        {
            var criteria = new AdminCheckInLicenseeCriteria
            {
                OnAdminId = internalLoginInfo.OnLoginUserId,
                LicenseeId = internalLoginInfo.LicenseeId
            };
            _internalService.AdminCheckInLicensee(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                return JsonResult(criteria.ErrCode);
            }

            if (CurrentSession.AgentInfo == null)
            {
                var agentInternalLoginCriteria = new InternalGetAgentInfoCriteria
                {
                    LicenseeId = internalLoginInfo.LicenseeId,
                    CurrencyId = internalLoginInfo.CurrencyId,
                    UserName = internalLoginInfo.AgentName
                };
                var agentInfo = _internalService.InternalGetAgentInfo(agentInternalLoginCriteria);
                CurrentSession.UpdateAgent(agentInfo);
                return JsonResult(Result.SuccessValue);
            }
            else
            {
                var authKey = _internalService.Encode(internalLoginInfo);
                var directUrl = string.Format("{0}?authKey={1}", Request.Url.AbsolutePath, authKey);
                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    DirectUrl = directUrl
                });
            }
        }

        [NonAction]
        public JsonResult LoginAsAgentk(InternalLoginInfo internalLoginInfo)
        {
            var authKey = _internalService.Encode(internalLoginInfo);
            var licenseDomainUrl = _coreService.GetLicenseeDomain(internalLoginInfo.LicenseeId);
            var directUrl = string.Format("{0}://{1}?authKey={2}", Request.Url.Scheme, Request.AppRelativeCurrentExecutionFilePath.Replace("~", licenseDomainUrl), authKey);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                DirectUrl = directUrl
            });
        }

        #endregion Internal login as agent
    }
}