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
    public class HomeController : BaseController
    {
        /// <summary>
        /// Index Action.
        /// </summary>
        /// <param name="act">The action.</param>
        /// <returns>
        /// Return Index page.
        /// </returns>
        [HttpGet]
        public ActionResult Index(string act)
        {
            var topMenu = TopMenu.Home;
            if (!string.IsNullOrEmpty(act))
            {
                switch (act.ToLower())
                {
                    case "changepassword":
                        topMenu = TopMenu.Security;
                        break;
                }
            }

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