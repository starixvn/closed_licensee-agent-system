// ----------------------------------------------------------------------
// <copyright file="NotifyController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Web.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Filters;

    [AllowAnonymous]
    public class NotifyController : Controller
    {
        /// <summary>
        /// Under Maintenance.
        /// </summary>
        /// <returns>View result.</returns>
        [AjaxOnly]
        [UnderMaintenance(Ignore = true)]
        public JsonResult UnderMaintenance()
        {
            var maintenanceService = new UnderMaintenanceService();
            var maintenanceInfo = maintenanceService.GetUnderMaintenanceInfo();
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Status = maintenanceInfo.Status
            });
        }
    }
}