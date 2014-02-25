// ----------------------------------------------------------------------
// <copyright file="NotifyController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Web.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AllowAnonymous]
    public class NotifyController : Controller
    {
        /// <summary>
        /// Under Maintenance.
        /// </summary>
        /// <returns>View result.</returns>
        [HttpGet]
        public ActionResult UnderMaintenance()
        {
            var maintenanceService = new UnderMaintenanceService();
            var maintenanceInfo = maintenanceService.GetUnderMaintenanceInfo();
            if (!maintenanceInfo.Status)
            {
                return Redirect(AppURLs.URLs.SignOut);
            }

            return View(maintenanceInfo);
        }
    }
}