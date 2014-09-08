namespace LicenseeAgent.Account.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Fanex.Security;
    using LicenseeAgent.Account.Criteria.Internal;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Account.ViewModels;
    using LicenseeAgent.API.LRF;
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

        #region Authentication

        [AdminSite]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("SignIn");
        }

        /// <summary>
        /// Signs in by IT/CSD.
        /// </summary>
        /// <returns>Returns MVC View.</returns>
        [AdminSite]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            var currentLanguage = Language;
            var model = new AdminSignInViewModel
            {
                Language = currentLanguage
            };

            return View(model);
        }

        /// <summary>
        /// SignIn Action.
        /// </summary>
        /// <param name="model">The MVC model.</param>
        /// <returns>Returns MVC View.</returns>
        [AdminSite]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(AdminSignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (AuthenticateUser(model))
                {
                    return Redirect(FormsAuthentication.DefaultUrl);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Validates the Admin user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns true whether username/password is valid.
        /// </returns>
        [NonAction]
        public bool AuthenticateUser(AdminSignInViewModel model)
        {
            var userSaltCriteria = new AdminSaltSelectCriteria
            {
                UserName = model.Username
            };

            var userSalt = _internalService.GetAdminSalt(userSaltCriteria);
            if (userSaltCriteria.ErrCode != Result.SuccessValue)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(userSaltCriteria.ErrCode));
                return false;
            }

            var clientInfo = Request.GetClientInfo();
            var criteria = new AdminLoginCriteria
            {
                UserName = model.Username,
                UserPwd = Cryptography.CreateHash(model.Password, userSalt),
                UserSalt = userSalt,
                LoginIp = clientInfo.Ip,
                BrowserName = clientInfo.BrowserName,
                Platform = clientInfo.Platform,
                CountryCode = clientInfo.CountryCode
            };

            var adminInfo = _internalService.AuthenticateAdmin(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                return false;
            }

            AdminCheckIn(adminInfo, model.Language);
            return true;
        }

        /// <summary>
        /// CSD/IT check-in when direct sign-in.
        /// </summary>
        /// <param name="adminInfo">The admin information.</param>
        /// <param name="language">The language.</param>
        [NonAction]
        public void AdminCheckIn(AdminInfo adminInfo, string language)
        {
            var criteria = new AdminCheckInCriteria
            {
                AdminId = adminInfo.UserId,
                IP = Request.GetClientIp(),
                NetSessionId = Session.SessionID
            };

            _internalService.AdminCheckIn(criteria);
            var currentSession = new CurrentSession(adminInfo, criteria.OnAdminId, language);
            SessionManager.CurrentSession = currentSession;
            FormsAuthentication.SetAuthCookie(adminInfo.UserName, true);
        }

        #endregion Authentication

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

            _internalService.AdminCheckIn(criteria);
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
        public JsonResult LoginAsUser(InternalLoginAsUserViewModel model)
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

        #region Menu to select licensee & currency

        /// <summary>
        /// Show menu to select licensee and currency and agent name.
        /// </summary>
        /// <returns>Partial view model.</returns>
        [ChildActionOnly]
        [HttpGet]
        [AppAuthorize(IsValidateAgent = false)]
        [AccountType(AccountType.Admin)]
        public ActionResult SelectMenu()
        {
            if (!CurrentSession.IsViewSelectMenu())
            {
                return Content("Error");
            }

            var model = new LicenseeViewModel();
            if (CurrentSession.IsViewSelectLicensee())
            {
                model.Licensees = _internalService.GetLicensees(CurrentSession.LoginUserId);
                if (CurrentSession.AgentInfo != null)
                {
                    model.LicenseeId = CurrentSession.AgentInfo.LicenseeId;
                    model.LicenseeName = model.Licensees.Where(x => x.LicenseeId == model.LicenseeId).Select(x => x.LicenseeName).FirstOrDefault();
                    model.CurrencyId = CurrentSession.AgentInfo.CurrencyId;
                    model.Currencies = _internalService.GetLicenseeCurrencies(new GetCurrencyOfLicenseeCriteria
                    {
                        UserId = CurrentSession.LoginUserId,
                        LicenseeId = model.LicenseeId
                    });
                    model.CurrencyName = model.Currencies.Where(x => x.Currency == model.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault();
                }
            }
            else
            {
                model.LicenseeId = CurrentSession.OnLicenseeId;
                model.Currencies = _internalService.GetLicenseeCurrencies(new GetCurrencyOfLicenseeCriteria
                {
                    UserId = CurrentSession.LoginUserId,
                    LicenseeId = model.LicenseeId
                });
                if (CurrentSession.AgentInfo != null)
                {
                    model.CurrencyId = CurrentSession.AgentInfo.CurrencyId;
                    model.CurrencyName = model.Currencies.Where(x => x.Currency == model.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault();
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// View data list currency.
        /// </summary>
        /// <param name="model">Get currency licensee criteria.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        [AppAuthorize(IsValidateAgent = false)]
        [AccountType(AccountType.Admin)]
        public JsonResult GetCurrencies(CurrencyLicenseeViewModel model)
        {
            var criteria = new GetCurrencyOfLicenseeCriteria
            {
                UserId = CurrentSession.LoginUserId,
                LicenseeId = model.LicenseeId
            };

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = _internalService.GetLicenseeCurrencies(criteria)
            });
        }

        #endregion Menu to select licensee & currency
    }
}