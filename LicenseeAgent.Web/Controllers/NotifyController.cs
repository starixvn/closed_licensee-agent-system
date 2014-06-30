namespace LicenseeAgent.Web.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// Notify controller.
    /// </summary>
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