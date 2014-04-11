// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Controllers
{
    using System.Web.Mvc;
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
        public ActionResult Example()
        {
            return this.View();
        }
    }
}