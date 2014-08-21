namespace LicenseeAgent.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.ViewModels;

    /// <summary>
    /// Authenticate for Agent user.
    /// </summary>
    [AppAuthorize(IsValidateAgent = false)]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Index Action.
        /// </summary>
        /// <returns>
        /// Return Index page.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            var topMenu = TopMenu.Home;
            var isAgentChangePassword = CurrentSession.IsAgentChangePassword();
            if (isAgentChangePassword)
            {
                topMenu = TopMenu.Security;
            }
            else
            {
                var topMenuCookie = Request.Cookies[Constants.AppCookieTopMenu];
                if (topMenuCookie != null)
                {
                    switch (topMenuCookie.Value)
                    {
                        case "security":
                            topMenu = TopMenu.Security;
                            break;

                        default:
                            break;
                    }

                    Response.Cookies[Constants.AppCookieTopMenu].Expires = DateTime.Now.AddDays(-1);
                }
            }

            var model = new HomeViewModel
            {
                TopMenu = topMenu,
                IsAgentChangePassword = isAgentChangePassword
            };

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Help()
        {
            return this.View();
        }

        [HttpGet]
        [AdminSite]
        [LocalRequest]
        public ActionResult Example()
        {
            return this.View();
        }
    }
}