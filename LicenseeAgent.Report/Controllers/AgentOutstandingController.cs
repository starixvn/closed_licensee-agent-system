namespace LicenseeAgent.Report.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Report.Criteria.AgentOutstanding;
    using LicenseeAgent.Report.Services;
    using LicenseeAgent.Report.Utilities;
    using LicenseeAgent.Report.ViewModels.AgentOutstanding;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// AgentOutstandingController class.
    /// </summary>
    public class AgentOutstandingController : BaseController
    {
        #region Variables

        private readonly IDownLinePermissionService _downLinePermissionService;
        private readonly IAgentOutstandingService _agentOutstandingService;
        private readonly IBetListService _betlistService;

        #endregion Variables

        #region Constructors

        public AgentOutstandingController()
            : this(null, null, null)
        {
        }

        public AgentOutstandingController(
            IDownLinePermissionService downLinePermissionService,
            IAgentOutstandingService agentOutstandingService,
            IBetListService betlistService)
        {
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _agentOutstandingService = agentOutstandingService ?? new AgentOutstandingService();
            _betlistService = betlistService ?? new BetListService();
        }

        #endregion Constructors

        #region Index

        /// <summary>
        /// Outstanding Report.
        /// </summary>
        /// <returns>Return a JSON object.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new AgentOutstandingViewModel
            {
                AgentId = CurrentSession.AgentInfo.AgentId
            };

            ViewBag.PageContext = new PageContext(CurrentSession);
            return View(model);
        }

        /// <summary>
        /// Outstanding Report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Return a JSON object.
        /// </returns>
        [HttpPost]
        [ActionName("Index")]
        public JsonResult IndexPost(AgentOutstandingViewModel model)
        {
            model.AgentId = model.AgentId == 0 ? CurrentSession.AgentInfo.AgentId : model.AgentId;
            if (model.AgentId != CurrentSession.AgentInfo.AgentId && !_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var agentOutstandings = _agentOutstandingService.GetAgentList(new AgentOutstandingReportCriteria
            {
                AgentId = model.AgentId
            });

            var memberOutstanding = _agentOutstandingService.GetMemberSum(new MemberOutstandingReportCriteria
            {
                AgentId = model.AgentId
            });

            if (memberOutstanding != null)
            {
                agentOutstandings.Add(memberOutstanding);
            }

            decimal sumOutstanding = agentOutstandings.Sum(i => i.Outstanding);
            decimal sumPositiontaking = agentOutstandings.Sum(i => i.Positiontaking);
            return Json(new
            {
                List = agentOutstandings,
                SumOutstanding = sumOutstanding,
                SumPositiontaking = sumPositiontaking
            });
        }

        #endregion Index

        #region BetList

        /// <summary>
        /// Outstanding - Bet List Report.
        /// </summary>
        /// <param name="criteria">Outstanding Report Criteria.</param>
        /// <returns>Return a JSON object.</returns>downlinemember
        [HttpPost]
        public JsonResult BetList(OutstandingReportBetListCriteria criteria)
        {
            if (!_downLinePermissionService.IsDownLineMember(criteria.CustId))
            {
                return RedirectToAccessDeniedJson();
            }

            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var ticketResult = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = ticketResult,
                IsAdmin = CurrentSession.IsAdmin,
                IsViewPlayerCommission = CurrentSession.IsViewPlayerCommission()
            };

            var betListTicketBuilder = new OutstandingBetListTicketBuilder(ticketBuilderParameters);
            var ticketTextList = betListTicketBuilder.BuildTickets();
            var subTotalWinLost = ticketResult.MainTicketList.Sum(x => x.Winlost);
            var subTotalComm = ticketResult.MainTicketList.Sum(x => x.PlayerComm);
            return Json(new
            {
                List = ticketTextList,
                TotalItem = ticketTextList.Count(),
                SubTotalWinLost = subTotalWinLost,
                SubTotalComm = subTotalComm,
                TotalWinLost = subTotalWinLost + subTotalComm,
                ErrCode = Result.SuccessValue
            });
        }

        #endregion BetList
    }
}