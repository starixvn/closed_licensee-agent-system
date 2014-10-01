namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public class TotalBetsController : BaseController
    {
        #region Variables

        private readonly IBetListService _betlistService;
        private readonly ICommonService _commonService;

        #endregion Variables

        #region Controllers

        public TotalBetsController()
            : this(null, null)
        {
        }

        public TotalBetsController(
            CommonService commonService,
            BetListService betlistService)
        {
            _commonService = commonService ?? new CommonService();
            _betlistService = betlistService ?? new BetListService();
        }

        #endregion Controllers

        #region Get leagues

        /// <summary>
        /// Get all Leagues.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// League list.
        /// </returns>
        [NonAction]
        public JsonResult GetLeagues(GetSportLeaguesViewModel model)
        {
            var criteria = new LeagueCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                SportType = model.SportType
            };
            var leagues = _commonService.GetLeagues(criteria).OrderBy(x => x.LeagueName).ToList();
            return GetLeagues(leagues, model.SelectedLeagues, model.CheckedAll);
        }

        /// <summary>
        /// Convert list league to JSON.
        /// </summary>
        /// <param name="leagues">List leagues.</param>
        /// <param name="selectedLeagues">Selected Leagues.</param>
        /// <param name="checkedAll">Checked All.</param>
        /// <returns>
        /// JSON list league.
        /// </returns>
        [NonAction]
        public JsonResult GetLeagues(IList<LeagueEntity> leagues, string selectedLeagues, bool checkedAll)
        {
            var leagueIds = string.IsNullOrEmpty(selectedLeagues) ? new List<string>() : selectedLeagues.Split(',').ToList();
            var leagueMultiItems = leagues.Select(x => new
            {
                id = x.LeagueId,
                value = x.LeagueId.ToString(),
                text = x.LeagueName,
                @checked = checkedAll || leagueIds.Contains(x.LeagueId.ToString()),
                enable = true
            });

            return Json(leagueMultiItems);
        }

        #endregion Get leagues

        #region Get events

        /// <summary>
        /// Get events by leagues.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Events result.
        /// </returns>
        [NonAction]
        public JsonResult GetEvents(GetSportEventsViewModel model)
        {
            var criteria = new EventCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                SportType = model.SportType,
                LeagueList = model.SelectedLeagues
            };
            var events = _commonService.GetEventsByLeague(criteria).OrderBy(x => x.MatchName).ToList();
            return GetEvents(events, model.SelectedEvents, model.CheckedAll);
        }

        /// <summary>
        /// Generate event list to JSON.
        /// </summary>
        /// <param name="events">All Events.</param>
        /// <param name="selectedEvents">Event selected list.</param>
        /// <param name="checkedAll">Checked All.</param>
        /// <returns>
        /// Return JSONRESULT.
        /// </returns>
        [NonAction]
        public JsonResult GetEvents(IList<EventEntity> events, string selectedEvents, bool checkedAll)
        {
            var eventIds = string.IsNullOrEmpty(selectedEvents) ? new List<string>() : selectedEvents.Split(',').ToList();
            var eventMultiItems = events.Select(x => new
            {
                id = x.MatchId,
                value = x.MatchId.ToString(),
                text = x.MatchName,
                @checked = checkedAll || eventIds.Contains(x.MatchId.ToString()),
                enable = true
            });

            return Json(eventMultiItems);
        }

        #endregion Get events

        #region Bet List

        /// <summary>
        /// Generate Bet List.
        /// </summary>
        /// <param name="criteria">Bet List By Match Criteria.</param>
        /// <returns>Return JSONRESULT.</returns>
        [HttpPost]
        public JsonResult BetList(BetListByMatchCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var ticketResult = _betlistService.GetBetList(criteria);
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
                TotalItem = criteria.TotalRows
            });
        }

        #endregion Bet List
    }
}