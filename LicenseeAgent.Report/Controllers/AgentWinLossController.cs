namespace LicenseeAgent.Report.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Report.Criteria.AgentWinLoss;
    using LicenseeAgent.Report.Services;
    using LicenseeAgent.Report.ViewModels.AgentWinLoss;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// Report Controller.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Review")]
    public class AgentWinLossController : BaseController
    {
        #region Variables

        private readonly IDownLinePermissionService _downLinePermissionService;
        private readonly ICoreService _coreService;
        private readonly IAgentWinLossService _agentWinLossService;

        #endregion Variables

        #region Constructors

        public AgentWinLossController()
            : this(null, null, null)
        {
        }

        public AgentWinLossController(
            IDownLinePermissionService downLinePermissionService,
            ICoreService coreService,
            IAgentWinLossService agentWinLossService)
        {
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _coreService = coreService ?? new CoreService();
            _agentWinLossService = agentWinLossService ?? new AgentWinLossService();
        }

        #endregion Constructors

        #region Index

        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <param name="model">Agent Win Loss Report View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Index(AgentWinLossViewModel model)
        {
            model.AgentId = model.AgentId > 0 ? model.AgentId : CurrentSession.AgentInfo.AgentId;
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedPage();
            }

            model.AgentLevel = model.AgentLevel > 0 ? model.AgentLevel : CurrentSession.AgentInfo.AgentLevel;
            model.UserName = !string.IsNullOrEmpty(model.UserName) ? model.UserName : CurrentSession.AgentInfo.UserName;
            model.BreadCrumb = !string.IsNullOrEmpty(model.BreadCrumb)
                                ? model.BreadCrumb
                                : model.AgentId + "," + model.AgentLevel + "," + model.UserName;
            model.ProductList = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            model.IsDeposit = CurrentSession.AgentInfo.IsDeposit;
            ViewBag.PageContext = new PageContext(CurrentSession, model.AgentId, model.AgentLevel, model.UserName);
            return View(model);
        }

        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <param name="model">Agent Win Loss Report View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        [ActionName("Index")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Fix later.")]
        public JsonResult IndexPost(AgentWinLossViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            CriteriaBase criteria = null;
            var balanceUpDay = _coreService.BalanceUpDay;
            if (ModelHelper.IsLastAgent(model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel))
            {
                criteria = new WinLossAgentViewMemberCriteria(
                    model.AgentId, model.FromDate, model.ToDate, balanceUpDay.MoveBetsDay, balanceUpDay.BalanceUpDay, (int)model.DrawFilter, model.Product);
            }
            else if (ModelHelper.IsAgentLevel0(model.AgentLevel))
            {
                criteria = new WinLossLicenseeViewAgentCriteria(
                    model.AgentId, model.FromDate, model.ToDate, balanceUpDay.MoveBetsDay, balanceUpDay.BalanceUpDay, (int)model.DrawFilter, model.Product);
            }
            else
            {
                criteria = new WinLossAgentViewAgentCriteria(
                    model.AgentId, model.FromDate, model.ToDate, balanceUpDay.MoveBetsDay, balanceUpDay.BalanceUpDay, (int)model.DrawFilter, model.Product);
            }

            var result = _agentWinLossService.GetData(criteria);
            return Json(result);
        }

        #endregion Index
    }
}