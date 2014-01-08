// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky" date="2013.3.29">Created</change>
//   <change who="Leo" date="2013.9.18">Change Captcha, Before login has support for multi-language</change>
// </history>

namespace LicenseeAgent.Web.Controllers
{
    using System.Threading;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// Authenticate for Agent user.
    /// </summary>
    [AppAuthorize(IsValidateAgent = false)]
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

            return this.View(topMenu);
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

        [HttpGet]
        public ActionResult Popup()
        {
            Thread.Sleep(3000);
            return this.View();
        }
    }
}