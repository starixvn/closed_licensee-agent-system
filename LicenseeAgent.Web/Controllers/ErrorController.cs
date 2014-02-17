// --------------------------------------------------------------------------
// <copyright file="ErrorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Security;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.ViewModels;

    /// <summary>
    /// Rendering Error pages.
    /// </summary>
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        /// <summary>
        /// Show error page.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>
        /// Returns HTML content.
        /// </returns>
        [HttpGet]
        public ActionResult Index(int? statusCode)
        {
            var model = new ErrorViewModel
            {
                StatusCode = statusCode ?? (int)HttpStatusCode.InternalServerError
            };

            return View(model);
        }

        /// <summary>
        /// Show under maintenance page.
        /// </summary>
        /// <returns>Returns HTML content.</returns>
        public ActionResult UnderMaintenance()
        {
            var maintenanceService = new UnderMaintenanceService();
            var maintenanceInfo = maintenanceService.GetUnderMaintenanceInfo();
            if (maintenanceInfo.Status)
            {
                return View(maintenanceInfo);
            }
            else
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }
        }

        [HttpGet]
        public ActionResult Unauthorized()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public ActionResult WhoAmI()
        {
            return View();
        }
    }
}