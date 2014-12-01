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
        #region Variables

        private IUnderMaintenanceService _maintenanceService;

        #endregion Variables

        #region Constructors

        public ErrorController()
            : this(null)
        {
        }

        public ErrorController(IUnderMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? new UnderMaintenanceService();
        }

        #endregion Constructors

        #region Default error page

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

        #endregion Default error page

        #region Unauthorized page

        [HttpGet]
        public ActionResult Unauthorized()
        {
            return View();
        }

        #endregion Unauthorized page

        #region Access denied page

        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }

        #endregion Access denied page

        #region Undermaintenance page

        /// <summary>
        /// Under maintenance page.
        /// </summary>
        /// <returns>Returns view of page.</returns>
        [HttpGet]
        [UnderMaintenance(Ignore = true)]
        public ActionResult UnderMaintenance()
        {
            var maintenanceInfo = _maintenanceService.UnderMaintenanceInfo;
            if (maintenanceInfo.Status)
            {
                return View(maintenanceInfo);
            }
            else
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }
        }

        #endregion Undermaintenance page

        #region No currency permission page

        [HttpGet]
        public ActionResult CurrencyPermissionDenied()
        {
            return View();
        }

        #endregion No currency permission page
    }
}