namespace LicenseeAgent.Authentication.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Fanex.Security;
    using LicenseeAgent.Authentication.Criteria;
    using LicenseeAgent.Authentication.Criteria.Admin;
    using LicenseeAgent.Authentication.Services;
    using LicenseeAgent.Authentication.ViewModels.Admin;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// AdminController class.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    public class AdminController : BaseController
    {
        #region Variables

        private readonly IAdminService _adminService;

        #endregion Variables

        #region Constructors

        public AdminController()
            : this(null)
        {
        }

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService ?? new AdminService();
        }

        #endregion Constructors

        #region Authentication

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

            var userSalt = _adminService.GetAdminSalt(userSaltCriteria);
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

            var adminInfo = _adminService.AuthenticateAdmin(criteria);
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

            _adminService.AdminCheckIn(criteria);
            var currentSession = new CurrentSession(adminInfo, criteria.OnAdminId, language);
            SessionManager.CurrentSession = currentSession;
            FormsAuthentication.SetAuthCookie(adminInfo.UserName, true);
        }

        #endregion Authentication
    }
}