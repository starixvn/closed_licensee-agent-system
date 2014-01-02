// --------------------------------------------------------------------------
// <copyright file="InternalController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Account
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Account.ViewModels;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// Internal Login Controller.
    /// </summary>
    [AppAuthorize(OnlyAdminSite = true)]
    [SessionState(SessionStateBehavior.Default)]
    public class InternalController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ICurrencyService _currencyService;

        public InternalController(IAccountService accountService, ICurrencyService currencyService)
        {
            _accountService = accountService;
            _currencyService = currencyService;
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
        /// <param name="language">The selected language.</param>
        /// <returns>Returns MVC View.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn(string language)
        {
            var model = new AdminSignInViewModel
            {
                Language = Language
            };

            return View(model);
        }

        /// <summary>
        /// SignIn Action.
        /// </summary>
        /// <param name="model">The MVC model.</param>
        /// <returns>Returns MVC View.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(AdminSignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var domainName = Request.Url.Host;
                if (AuthenticateUser(model, domainName))
                {
                    return Redirect(FormsAuthentication.DefaultUrl);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Checks the user is valid before make a internal login.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON.</returns>
        [HttpPost]
        [AppAuthorize(Users = Constants.AdminUserOnly, OnlyAuthorize = true)]
        public JsonResult LoginAsUser(InternalLoginAsUserViewModel model)
        {
            var criteria = new InternalLoginAsUserCriteria
            {
                LicenseeId = model.LicenseeId,
                CurrencyId = model.CurrencyId,
                UserName = model.UserName
            };

            _accountService.InternalLoginAsUser(criteria);
            if (criteria.MsgId == 0)
            {
                var internalLoginInfo = new InternalLoginInfo
                {
                    LicenseeId = criteria.LicenseeId,
                    CurrencyId = criteria.CurrencyId,
                    AgentName = model.UserName,
                    Language = CurrentSession.Language,
                    UserId = CurrentSession.AdminInfo.UserId,
                    OnUserId = CurrentSession.OnUserId,
                };

                var agentInfo = criteria.CreateAgent();
                if (agentInfo.IsLevel0)
                {
                    return AdminLoginAsAgent0(internalLoginInfo, agentInfo);
                }
                else
                {
                    return AdminLoginAsAgent(internalLoginInfo);
                }
            }

            return JsonResult(criteria.MsgId);
        }

        /// <summary>
        /// Admins the login as agent level 0.
        /// </summary>
        /// <param name="internalLoginInfo">The admin internal login information.</param>
        /// <param name="agentInfo">The agent information.</param>
        /// <returns>Returns JSON result.</returns>
        [NonAction]
        public JsonResult AdminLoginAsAgent0(InternalLoginInfo internalLoginInfo, AgentInfo agentInfo)
        {
            // check in selected licensee
            var criteria = new AdminCheckInLicenseeCriteria
            {
                OnAdminId = internalLoginInfo.OnUserId,
                LicenseeId = internalLoginInfo.LicenseeId
            };

            _accountService.AdminCheckInLicensee(criteria);
            if (criteria.MsgId == Result.SuccessValue)
            {
                CurrentSession.AgentInfo = agentInfo;
                return Json(
                    new
                    {
                        MsgId = Result.SuccessValue
                    });
            }

            return JsonResult(criteria.MsgId);
        }

        /// <summary>
        /// Logins as agent level k > 0.
        /// </summary>
        /// <param name="internalLoginInfo">The internal login information.</param>
        /// <returns>
        /// Returns JSON result.
        /// </returns>
        [NonAction]
        public JsonResult AdminLoginAsAgent(InternalLoginInfo internalLoginInfo)
        {
            var reloadAdminSession = true;
            if (CurrentSession.AgentInfo != null &&
                CurrentSession.AgentInfo.LicenseeId != internalLoginInfo.LicenseeId &&
                CurrentSession.AgentInfo.CurrencyId != internalLoginInfo.CurrencyId)
            {
                reloadAdminSession = false;
            }

            if (reloadAdminSession)
            {
                var criteria = new InternalLoginAsUserCriteria
                {
                    LicenseeId = internalLoginInfo.LicenseeId,
                    CurrencyId = internalLoginInfo.CurrencyId
                };
                _accountService.InternalLoginAsUser(criteria);
                if (criteria.MsgId == Result.SuccessValue)
                {
                    var agentInfo = criteria.CreateAgent();

                    // Recheck in new agent level 0
                    AdminLoginAsAgent0(internalLoginInfo, agentInfo);
                }
                else
                {
                    return JsonResult(criteria.MsgId);
                }
            }

            var authKey = internalLoginInfo.Save();
            var ticket = new FormsAuthenticationTicket(
                1,
                CurrentSession.AdminInfo.UserName,
                DateTime.Now,
                DateTime.Now.AddSeconds(10),
                false,
                authKey);
            var authTicket = FormsAuthentication.Encrypt(ticket);
            var directUrl = string.Format("{0}/?authTicket={1}", Request.Url.AbsoluteUri, authTicket);
            return Json(
                new
                {
                    MsgId = Result.SuccessValue,
                    DirectUrl = directUrl
                });
        }

        /// <summary>
        /// Update user role.
        /// </summary>
        /// <param name="authTicket">The authentication ticket.</param>
        /// <returns>
        /// JSON null when update user role.
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoginAsUser(string authTicket)
        {
            if (string.IsNullOrEmpty(authTicket))
            {
                return Content("Ticket is not found");
            }

            var ticket = FormsAuthentication.Decrypt(authTicket);
            if (ticket.Expired)
            {
                return Content("Ticket is expired");
            }

            var internalLoginInfo = InternalLoginInfo.GetInfo(ticket.UserData);
            if (internalLoginInfo == null)
            {
                return Content("Data is invalid or it was used");
            }

            var criteria = new InternalLoginAsUserCriteria
            {
                LicenseeId = internalLoginInfo.LicenseeId,
                CurrencyId = internalLoginInfo.CurrencyId,
                UserName = internalLoginInfo.AgentName
            };
            _accountService.InternalLoginAsUser(criteria);
            if (criteria.MsgId == Result.SuccessValue)
            {
                var agentInfo = criteria.CreateAgent();
                return AdminLoginAsAgent(agentInfo, internalLoginInfo);
            }
            else
            {
                return Content(ResourceManager.GetErrorMsg(criteria.MsgId));
            }
        }

        [NonAction]
        public ActionResult AdminLoginAsAgent(AgentInfo agentInfo, InternalLoginInfo internalLoginInfo)
        {
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
        [AppAuthorize(Users = Constants.AdminUserOnly, OnlyAuthorize = true)]
        public ActionResult SelectMenu()
        {
            var model = new LicenseeViewModel
            {
                Licensees = _accountService.GetLicenseeList(new GetLicenseeListCriteria())
            };

            if (CurrentSession.IsViewSelectMenu())
            {
                if (CurrentSession.AgentInfo != null)
                {
                    model.LicenseeId = CurrentSession.AgentInfo.LicenseeId;
                    model.LicenseeName = model.Licensees.Where(x => x.LicenseeId == model.LicenseeId).Select(x => x.LicenseeName).FirstOrDefault();
                    model.CurrencyId = CurrentSession.AgentInfo.CurrencyId;
                    model.Currencies = _currencyService.GetCurrencyList(new GetCurrencyOfLicenseeCriteria
                    {
                        LicenseeId = model.LicenseeId
                    });
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
        [AppAuthorize(Users = Constants.AdminUserOnly, OnlyAuthorize = true)]
        public JsonResult GetCurrencies(CurrencyLicenseeViewModel model)
        {
            var criteria = new GetCurrencyOfLicenseeCriteria
            {
                LicenseeId = model.LicenseeId
            };

            return Json(new
            {
                MsgId = Result.SuccessValue,
                List = _currencyService.GetCurrencyList(criteria)
            });
        }

        /// <summary>
        /// Validates the Admin user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <returns>
        /// Returns true whether username/password is valid.
        /// </returns>
        private bool AuthenticateUser(AdminSignInViewModel model, string domainName)
        {
            var userSaltCriteria = new AdminSaltSelCriteria
            {
                UserName = model.Username
            };

            var userSalt = _accountService.GetAdminSalt(userSaltCriteria);
            if (userSaltCriteria.MsgId != 0)
            {
                ModelState.AddModelError(string.Empty, GetErrorMsg(userSaltCriteria.MsgId));
                return false;
            }

            var clientInfo = Request.GetClientInfo();
            var criteria = new AdminLoginCriteria
            {
                UserName = model.Username,
                UserPwd = model.Password,
                UserSalt = userSalt,
                LoginIp = clientInfo.Ip,
                LoginDomain = domainName,
                BrowserName = clientInfo.BrowserName,
                Platform = clientInfo.Platform,
                CountryCode = clientInfo.CountryCode
            };

            var adminInfo = _accountService.AuthenticateAdmin(criteria);
            if (criteria.MsgId != Result.SuccessValue)
            {
                ModelState.AddModelError(string.Empty, GetErrorMsg(criteria.MsgId));
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
    }
}