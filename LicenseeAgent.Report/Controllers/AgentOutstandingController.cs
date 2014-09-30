namespace LicenseeAgent.Report.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Report.Criteria.AgentOutstanding;
    using LicenseeAgent.Report.Services;
    using LicenseeAgent.Report.Utilities;
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
        private readonly ICoreService _coreService;
        private readonly IAgentOutstandingService _agentOutstandingService;
        private readonly IBetListService _betlistService;

        #endregion Variables

        #region Constructors

        public AgentOutstandingController()
            : this(null, null, null, null)
        {
        }

        public AgentOutstandingController(
            IDownLinePermissionService downLinePermissionService,
            ICoreService coreService,
            IAgentOutstandingService agentOutstandingService,
            IBetListService betlistService)
        {
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _coreService = coreService ?? new CoreService();
            _agentOutstandingService = agentOutstandingService ?? new AgentOutstandingService();
            _betlistService = betlistService ?? new BetListService();
        }

        #endregion Constructors

        #region Actions

        /// <summary>
        /// Outstanding Report.
        /// </summary>
        /// <returns>Return a JSON object.</returns>
        public ActionResult Index()
        {
            ViewBag.PageContext = new PageContext(CurrentSession);
            return View();
        }

        /// <summary>
        /// Outstanding Report.
        /// </summary>
        /// <param name="criteria">Outstanding Report Criteria.</param>
        /// <returns>Return a JSON object.</returns>
        [HttpPost]
        public JsonResult Index(OutstandingReportCriteria criteria)
        {
            if (criteria.AgentId != -1 && !_downLinePermissionService.IsDownLineAgent(criteria.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            criteria.AgentId = criteria.AgentId <= 0 ? CurrentSession.AgentInfo.AgentId : criteria.AgentId;
            var agentOutstandings = _agentOutstandingService.GetData(criteria);

            decimal sumOutstanding = agentOutstandings.Sum(i => i.Outstanding);
            decimal sumPositiontaking = agentOutstandings.Sum(i => i.Positiontaking);

            return Json(new
            {
                List = agentOutstandings.ToList(),
                SumOutstanding = sumOutstanding,
                SumPositiontaking = sumPositiontaking,
                IsLastLevel = criteria.IsLastLevel,
                ErrCode = Result.SuccessValue
            });
        }

        #endregion Actions

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