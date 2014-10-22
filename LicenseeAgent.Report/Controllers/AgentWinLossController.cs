namespace LicenseeAgent.Report.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Report.Criteria.AgentWinLoss;
    using LicenseeAgent.Report.Entities.AgentWinLoss;
    using LicenseeAgent.Report.Services;
    using LicenseeAgent.Report.ViewModels.AgentWinLoss;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
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
        /// <returns>
        /// Return a view.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new AgentWinLossViewModel
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                ShowMembers = CurrentSession.AgentInfo.IsLastLevel,
                ProductList = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId)
            };

            return View(model);
        }

        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <param name="model">Agent Win Loss Report View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        [ActionName("Index")]
        public JsonResult IndexPost(AgentWinLossViewModel model)
        {
            model.AgentId = model.AgentId == 0 ? CurrentSession.AgentInfo.AgentId : model.AgentId;
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var agentInfo = _coreService.GetAgentInfo(model.AgentId);
            model.AgentLevel = agentInfo.AgentLevel;
            var balanceUpDay = _coreService.BalanceUpDay;
            var memberCriteria = new WinLossMemberViewCriteria(
                model.AgentId,
                model.FromDate,
                model.ToDate,
                balanceUpDay.MoveBetsDay,
                balanceUpDay.BalanceUpDay,
                (int)model.DrawFilter,
                model.Product);
            var list = new List<AgentWinLossEntity>();
            if (!model.ShowMembers)
            {
                var listCriteria = GetCriteria(model, balanceUpDay);
                list.AddRange(_agentWinLossService.GetAgentList(listCriteria));
                var memberSum = _agentWinLossService.GetMemberSum(memberCriteria);
                if (memberSum != null)
                {
                    list.Add(memberSum);
                }
            }
            else
            {
                list.AddRange(_agentWinLossService.GetMemberList(memberCriteria));
            }

            return Json(new
            {
                IsDeposit = CurrentSession.AgentInfo.IsDeposit,
                AgentName = string.IsNullOrEmpty(agentInfo.UserName) ? CurrentSession.AgentInfo.LicenseeName : agentInfo.UserName,
                AgentLevel = agentInfo.AgentLevel,
                NumberOfLevel = CurrentSession.AgentInfo.NumberOfLevel,
                List = list
            });
        }

        /// <summary>
        /// Gets the criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="balanceUpDay">The balance up day.</param>
        /// <returns>Returns criteria.</returns>
        [NonAction]
        public CriteriaBase GetCriteria(AgentWinLossViewModel model, SelectBalanceUpDay balanceUpDay)
        {
            if (ModelHelper.IsAgentLevel0(model.AgentLevel))
            {
                return new WinLossLicenseeViewCriteria(
                    model.AgentId, model.FromDate, model.ToDate, balanceUpDay.MoveBetsDay, balanceUpDay.BalanceUpDay, (int)model.DrawFilter, model.Product);
            }
            else
            {
                return new WinLossAgentViewCriteria(
                    model.AgentId, model.FromDate, model.ToDate, balanceUpDay.MoveBetsDay, balanceUpDay.BalanceUpDay, (int)model.DrawFilter, model.Product);
            }
        }

        #endregion Index
    }
}