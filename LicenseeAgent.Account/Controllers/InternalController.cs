// --------------------------------------------------------------------------
// <copyright file="InternalController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------


namespace LicenseeAgent.Account
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Account.ViewModels;
    using LicenseeAgent.API.LRF;
    using LicenseeAgent.Core.Criterias;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// Internal Login Controller.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    public class InternalController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ICurrencyService _currencyService;
        private readonly IInternalService _internalService;

        public InternalController(
            IAccountService accountService,
            ICurrencyService currencyService,
            IInternalService internalService,
            IDownlLinePermissionService permissionService)
            : base(permissionService)
        {
            _accountService = accountService;
            _currencyService = currencyService;
            _internalService = internalService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return this.RedirectToAction("SignIn");
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
        /// Internal login from LRF.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>Returns a page after login.</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Lrf(string authKey)
        {
            var loginInfo = Integration.Decrypt(authKey);
            if (loginInfo.IsExpired)
            {
                return Content(Las.SessionExpired);
            }

            var clientInfo = Request.GetClientInfo();
            var criteria = new LicenseeAdminLoginCriteria
            {
                LicenseeId = loginInfo.LicenseeId,
                UserId = loginInfo.UserId,
                LoginIp = clientInfo.Ip,
                BrowserName = clientInfo.BrowserName,
                Platform = clientInfo.Platform,
                CountryCode = clientInfo.CountryCode
            };
            _accountService.LicenseeAdminLogin(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                return Content(GetErrMsg(criteria.ErrCode));
            }

            var licenseeInfo = new LicenseeInfo
            {
                LicenseeId = criteria.LicenseeId,
                UserId = criteria.UserId,
                UserName = criteria.UserName
            };

            CheckIn(licenseeInfo, loginInfo.Language);
            return Redirect(FormsAuthentication.DefaultUrl);
        }

        /// <summary>
        /// Checks the user is valid before make a internal login.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON.</returns>
        [HttpPost]
        [AppAuthorize(Role = AccountType.Admin, IsValidateAgent = false)]
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

            _accountService.InternalLoginAsUser(criteria);
            if (criteria.ErrCode == Result.SuccessValue)
            {
                var internalLoginInfo = new InternalLoginInfo
                {
                    LoginUserId = CurrentSession.LoginUserId,
                    LoginAccountType = CurrentSession.LoginAccountType,
                    LoginUserName = CurrentSession.LoginUserName,
                    OnLoginUserId = CurrentSession.OnUserId,
                    Language = CurrentSession.Language,
                    LicenseeId = criteria.LicenseeId,
                    CurrencyId = criteria.CurrencyId,
                    AgentName = model.UserName
                };

                return InternalLogin(internalLoginInfo);
            }

            return JsonResult(criteria.ErrCode);
        }

        /// <summary>
        /// Admins direct login as agent.
        /// </summary>
        /// <param name="internalLoginInfo">The admin internal login information.</param>
        /// <returns>
        /// Returns JSON result.
        /// </returns>
        [NonAction]
        public JsonResult InternalLogin(InternalLoginInfo internalLoginInfo)
        {
            if (string.IsNullOrEmpty(internalLoginInfo.AgentName))
            {
                var criteria = new AdminCheckInLicenseeCriteria
                {
                    OnAdminId = internalLoginInfo.OnLoginUserId,
                    LicenseeId = internalLoginInfo.LicenseeId
                };
                _accountService.AdminCheckInLicensee(criteria);
                if (criteria.MsgId != Result.SuccessValue)
                {
                    return JsonResult(criteria.MsgId);
                }
            }

            if (CurrentSession.AgentInfo == null)
            {
                var criteria = new AgentInternalLoginCriteria
                {
                    LicenseeId = internalLoginInfo.LicenseeId,
                    CurrencyId = internalLoginInfo.CurrencyId,
                    UserName = internalLoginInfo.AgentName
                };
                var agentInfo = _internalService.AgentInternalLogin(criteria);
                CurrentSession.UpdateAgent(agentInfo);
                return JsonResult(Result.SuccessValue);
            }
            else
            {
                var authKey = _internalService.Encode(internalLoginInfo);
                var directUrl = string.Format("{0}/?authKey={1}", Request.Url.AbsolutePath, authKey);
                return Json(
                    new
                    {
                        ErrCode = Result.SuccessValue,
                        DirectUrl = directUrl
                    });
            }
        }

        /// <summary>
        /// Update user role.
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

            return AdminLoginAsAgent(internalLoginInfo);
        }

        /// <summary>
        /// Admins the login as agent.
        /// </summary>
        /// <param name="internalLoginInfo">The internal login information.</param>
        /// <returns>Returns page after internal login.</returns>
        [NonAction]
        public ActionResult AdminLoginAsAgent(InternalLoginInfo internalLoginInfo)
        {
            var criteria = new AgentInternalLoginCriteria
            {
                LicenseeId = internalLoginInfo.LicenseeId,
                CurrencyId = internalLoginInfo.CurrencyId,
                UserName = internalLoginInfo.AgentName
            };
            var agentInfo = _internalService.AgentInternalLogin(criteria);
            SessionManager.CurrentSession = new CurrentSession(agentInfo, internalLoginInfo);
            FormsAuthentication.SetAuthCookie(agentInfo.UserName, true);
            return Redirect(FormsAuthentication.DefaultUrl);
        }

        /// <summary>
        /// Show menu to select licensee and currency and agent name.
        /// </summary>
        /// <returns>Partial view model.</returns>
        [ChildActionOnly]
        [HttpGet]
        [AppAuthorize(Role = AccountType.Admin, IsValidateAgent = false)]
        public ActionResult SelectMenu()
        {
            if (!CurrentSession.IsViewSelectMenu())
            {
                return Content("Error");
            }

            var model = new LicenseeViewModel();
            if (CurrentSession.LoginAccountType == AccountType.CSD_IT)
            {
                model.Licensees = _accountService.GetLicenseeList(new GetLicenseeListCriteria
                {
                    UserId = CurrentSession.LoginUserId
                });
                if (CurrentSession.AgentInfo != null)
                {
                    model.LicenseeId = CurrentSession.AgentInfo.LicenseeId;
                    model.LicenseeName = model.Licensees.Where(x => x.LicenseeId == model.LicenseeId).Select(x => x.LicenseeName).FirstOrDefault();
                    model.CurrencyId = CurrentSession.AgentInfo.CurrencyId;
                    model.Currencies = _currencyService.GetCurrencyList(new GetCurrencyOfLicenseeCriteria
                    {
                        UserId = CurrentSession.LoginUserId,
                        LicenseeId = model.LicenseeId
                    });
                    model.CurrencyName = model.Currencies.Where(x => x.Currency == model.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault();
                }
            }
            else if (CurrentSession.LoginAccountType == AccountType.Licensee)
            {
                model.LicenseeId = CurrentSession.OnLicenseeId;
                model.Currencies = _currencyService.GetCurrencyList(new GetCurrencyOfLicenseeCriteria
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
        [AppAuthorize(Role = AccountType.Admin, IsValidateAgent = false)]
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
                List = _currencyService.GetCurrencyList(criteria)
            });
        }

        /// <summary>
        /// Validates the Admin user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns true whether username/password is valid.
        /// </returns>
        private bool AuthenticateUser(AdminSignInViewModel model)
        {
            var userSaltCriteria = new AdminSaltSelCriteria
            {
                UserName = model.Username
            };

            var userSalt = _accountService.GetAdminSalt(userSaltCriteria);
            if (userSaltCriteria.MsgId != 0)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(userSaltCriteria.MsgId));
                return false;
            }

            var clientInfo = Request.GetClientInfo();
            var criteria = new AdminLoginCriteria
            {
                UserName = model.Username,
                UserPwd = model.Password,
                UserSalt = userSalt,
                LoginIp = clientInfo.Ip,
                BrowserName = clientInfo.BrowserName,
                Platform = clientInfo.Platform,
                CountryCode = clientInfo.CountryCode
            };

            var adminInfo = _accountService.AuthenticateAdmin(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                return false;
            }
            else
            {
                CheckIn(adminInfo, model.Language);
                return true;
            }
        }

        /// <summary>
        /// Save current user session.
        /// </summary>
        /// <param name="adminInfo">The admin information.</param>
        /// <param name="language">The language.</param>
        private void CheckIn(AdminInfo adminInfo, string language)
        {
            var criteria = new AdminCheckInCriteria
            {
                AdminId = adminInfo.UserId,
                Ip = Request.GetClientIp(),
                NetSessionId = Session.SessionID
            };

            _accountService.AdminCheckIn(criteria);
            var currentSession = new CurrentSession(adminInfo, criteria.OnAdminId, language);
            SessionManager.CurrentSession = currentSession;
            FormsAuthentication.SetAuthCookie(adminInfo.UserName, true);
        }

        /// <summary>
        /// Checks the in.
        /// </summary>
        /// <param name="licenseeInfo">The licensee information.</param>
        /// <param name="language">The language.</param>
        private void CheckIn(LicenseeInfo licenseeInfo, string language)
        {
            var criteria = new AdminCheckInCriteria
            {
                AdminId = licenseeInfo.UserId,
                Ip = Request.GetClientIp(),
                NetSessionId = Session.SessionID,
                LicenseeId = licenseeInfo.LicenseeId
            };

            _accountService.AdminCheckIn(criteria);
            var currentSession = new CurrentSession(licenseeInfo, criteria.OnAdminId, language);
            SessionManager.CurrentSession = currentSession;
            FormsAuthentication.SetAuthCookie(licenseeInfo.UserName, true);
        }
    }
}