namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public class CommonController : BaseController
    {
        private readonly IBetListService _betlistService;

        private readonly ICommonService _commonService;

        public CommonController()
            : this(null, null)
        {
        }

        public CommonController(
            CommonService totalBetsService,
            BetListService betlistService)
        {
            _commonService = totalBetsService ?? new CommonService();
            _betlistService = betlistService ?? new BetListService();
        }

        /// <summary>
        /// Generate Bet List.
        /// </summary>
        /// <param name="criteria">Bet List By Match Criteria.</param>
        /// <returns>Return JSONRESULT.</returns>
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
                ErrCode = Result.SuccessValue,
                List = ticketTextList,
                TotalItem = criteria.TotalRows
            });
        }

        /// <summary>
        /// Get All League.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <param name="sportType">Sport Type.</param>
        /// <param name="checkedAll">If set to <c>true</c> [checked all].</param>
        /// <returns>
        /// League list.
        /// </returns>
        [HttpPost]
        public JsonResult GetLeagueList(string league, int sportType, bool checkedAll)
        {
            var criteria = new LeagueCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportType = sportType };
            var leagues = _commonService.GetLeagues(criteria).OrderBy(x => x.LeagueName);
            return LeagueListToJson(league, checkedAll, leagues);
        }

        /// <summary>
        /// Get events by leagues.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <param name="event">Event value.</param>
        /// <param name="sportType">Sport Type.</param>
        /// <param name="checkedAll">If set to <c>true</c> [checked all].</param>
        /// <returns>
        /// Events result.
        /// </returns>
        [HttpPost]
        public JsonResult GetEventsByLeagues(string league, string @event, int sportType, bool checkedAll)
        {
            var criteria = new EventCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportType = sportType, LeagueList = league };
            var events = _commonService.GetEventsByLeague(criteria).OrderBy(x => x.MatchName);
            return EventListToJson(@event, checkedAll, events);
        }

        /// <summary>
        /// Generate event list to JSON.
        /// </summary>
        /// <param name="event">Event selected list.</param>
        /// <param name="checkedAll">Checked All.</param>
        /// <param name="events">All Events.</param>
        /// <returns>Return JSONRESULT.</returns>
        protected JsonResult EventListToJson(string @event, bool checkedAll, IOrderedEnumerable<EventEntity> events)
        {
            var eventIds = string.IsNullOrEmpty(@event) ? new List<string>() : @event.Split(',').ToList();
            var eventMultiItems = new List<dynamic>();
            foreach (var item in events)
            {
                eventMultiItems.Add(new
                {
                    id = item.MatchId,
                    value = item.MatchId,
                    text = item.MatchName,
                    @checked = checkedAll || eventIds.Contains(item.MatchId.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = eventMultiItems,
            });
        }

        /// <summary>
        /// Convert list league to JSON.
        /// </summary>
        /// <param name="selectedLeagues">Selected Leagues.</param>
        /// <param name="checkedAll">Checked All.</param>
        /// <param name="leagues">List leagues.</param>
        /// <returns>JSON list league.</returns>
        protected JsonResult LeagueListToJson(string selectedLeagues, bool checkedAll, IEnumerable<LeagueEntity> leagues)
        {
            var leagueIds = string.IsNullOrEmpty(selectedLeagues) ? new List<string>() : selectedLeagues.Split(',').ToList();
            var leagueMultiItems = new List<dynamic>();
            foreach (var item in leagues)
            {
                leagueMultiItems.Add(new
                {
                    id = item.LeagueId,
                    value = item.LeagueId,
                    text = item.LeagueName,
                    @checked = checkedAll || leagueIds.Contains(item.LeagueId.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = leagueMultiItems,
            });
        }
    }
}