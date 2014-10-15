namespace LicenseeAgent.Web.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Web.Criteria.Common;
    using LicenseeAgent.Web.Services;
    using LicenseeAgent.Web.ViewModels.Common;

    /// <summary>
    /// CommonController class.
    /// </summary>
    public class CommonController : BaseController
    {
        #region Variables

        private readonly ICommonService _commonService;

        #endregion Variables

        #region Constructors

        public CommonController()
            : this(null)
        {
        }

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService ?? new CommonService();
        }

        #endregion Constructors

        #region IP

        /// <summary>
        /// Get IP Information.
        /// </summary>
        /// <param name="model">IP view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult IP(IpInformationViewModel model)
        {
            var criteria = new GetIpInformationCriteria { Ip = model.IpAddress };
            var ipInformation = _commonService.GetIpInformation(criteria);
            return View(new IpInformationViewModel
            {
                IpAddress = model.IpAddress,
                City = ipInformation.IpCity,
                Country = ipInformation.CountryLong
            });
        }

        #endregion IP
    }
}