// --------------------------------------------------------------------------
// <copyright file="ErrorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis" date="2013.5.15">Created</change>
// </history>

namespace LicenseeAgent.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Security;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Web.Filters;
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
            if (statusCode == null)
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
            }

            var url = statusCode.Value == (int)HttpStatusCode.InternalServerError ?
                            "~/Views/Error/Error.cshtml" : "~/Views/Error/Index.cshtml";
            var message = AppHandler.GetHttpErrorMessage(statusCode.Value);
            var model = new ErrorViewModel
            {
                ErrMsg = message
            };
            return View(url, model);
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
        public ActionResult AccountStopped()
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