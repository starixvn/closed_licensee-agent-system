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
        #region Variables

        private readonly IUnderMaintenanceService _maintenanceService;

        #endregion Variables

        #region Constructors

        public NotifyController()
            : this(null)
        {
        }

        public NotifyController(IUnderMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? new UnderMaintenanceService();
        }

        #endregion Constructors

        #region Under maintenance

        /// <summary>
        /// Under Maintenance.
        /// </summary>
        /// <returns>View result.</returns>
        [AjaxOnly]
        [UnderMaintenance(Ignore = true)]
        public JsonResult UnderMaintenance()
        {
            var maintenanceInfo = _maintenanceService.UnderMaintenanceInfo;
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Status = maintenanceInfo.Status
            });
        }

        #endregion Under maintenance
    }
}