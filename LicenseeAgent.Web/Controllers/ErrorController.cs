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
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.ViewModels;

    /// <summary>
    /// Rendering Error pages.
    /// </summary>
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        private IUnderMaintenanceService _maintenanceService;

        public ErrorController()
            : this(null)
        {
        }

        public ErrorController(IUnderMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? new UnderMaintenanceService();
        }

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

        /// <summary>
        /// Under maintenance page.
        /// </summary>
        /// <returns>Returns view of page.</returns>
        [HttpGet]
        [UnderMaintenance(Ignore = true)]
        public ActionResult UnderMaintenance()
        {
            var maintenanceInfo = _maintenanceService.GetUnderMaintenanceInfo();
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
        public ActionResult CurrencyPermissionDenied()
        {
            return View();
        }
    }
}