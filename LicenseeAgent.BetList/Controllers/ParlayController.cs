namespace LicenseeAgent.BetList.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Entities;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Entities;
    using Services;

    /// <summary>
    /// ParlayController class.
    /// </summary>
    public class ParlayController : BetListBase
    {
        private readonly IBetListService _betlistService;

        public ParlayController()
            : this(null)
        {
        }

        public ParlayController(
            IBetListService betlistService)
        {
            _betlistService = betlistService ?? new BetListService();
        }

        [HttpGet]
        public ActionResult ParlayList()
        {
            ViewBag.PageContext = new PageContext(CurrentSession, 0);
            return View("~/Views/BetList/ParlayList.cshtml");
        }

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="criteria">Bet List By Member Criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult ParlayList(TotalBetsParlayListCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var ticketResult = _betlistService.GetParlayList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = ticketResult,
                IsAdmin = CurrentSession.IsAdmin,
                IsViewPlayerCommission = CurrentSession.IsViewPlayerCommission()
            };
            var betListTicketBuilder = new TotalBetsTicketBuilder(ticketBuilderParameters);
            var ticketTextList = betListTicketBuilder.BuildTickets();
            return Json(new
            {
                List = ticketTextList,
                TotalItem = ticketTextList.Count
            });
        }
    }
}