// ----------------------------------------------------------------------
// <copyright file="TotalBetsController.Soccer.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.Utility;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public partial class TotalBetsController : BaseController
    {
        private readonly ITotalBetsService _totalBetsService;

        private readonly IBetListService _betlistService;

        public TotalBetsController(
            TotalBetsService totalBetsService,
            BetListService betlistService,
            IDownlLinePermissionService permissionService)
            : base(permissionService)
        {
            _totalBetsService = totalBetsService;
            _betlistService = betlistService;
        }

        [HttpGet]
        public ViewResult Soccer(SoccerViewModel model)
        {
            model.TimeInterval = model.TimeInterval > 0 ? model.TimeInterval : 15;
            return View(model);
        }

        /// <summary>
        /// Soccer the detail.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Detail of soccer page.</returns>
        [HttpGet]
        public ViewResult SoccerDetail(SoccerDetailViewModel model)
        {
            var criteria = new SoccerDetailCriteria { AgentId = CurrentSession.AgentInfo.AgentId, MatchId = model.MatchId };
            var dataList = _totalBetsService.GetSoccerDetailData(criteria);
            model.MainData = dataList.ToList();
            return View(model);
        }

        /// <summary>
        /// Get soccer data.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Soccer data.</returns>
        [HttpPost]
        public JsonResult GetSoccerData(SoccerCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var dataList = _totalBetsService.GetSoccerData(criteria);
            var mapData = TotalBetsUtility.MapTotalBetsMainData(dataList)
                .OrderByDescending(x => x.LiveIndicator)
                .ThenBy(x => x.LeagueName)
                .ToList();
            if (mapData.Count == 2 && mapData[0].LeagueId == mapData[1].LeagueId)
            {
                foreach (var match in mapData[1].MatchList)
                {
                    mapData[0].MatchList.Add(match);
                }

                mapData.RemoveAt(1);
            }

            return Json(new
            {
                List = mapData
            });
        }

        /// <summary>
        /// Gets the soccer bet list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns JSON object.</returns>
        [HttpPost]
        public JsonResult GetSoccerBetList(BetListByMatchCriteria criteria)
        {
            return BetList(criteria);
        }

        [HttpGet]
        public ViewResult SoccerBetList(BetListByMatchCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            return View(criteria);
        }

        /// <summary>
        /// Gets the soccer more bet list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns JSON object.</returns>
        [HttpPost]
        public JsonResult GetSoccerMoreBetList(BetListByMatchCriteria criteria)
        {
            criteria.ShowLiveTicket = null;
            return BetList(criteria);
        }

        #region Utilities

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
            var leagues = _totalBetsService.GetLeagues(criteria).OrderBy(x => x.LeagueName);
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
            var events = _totalBetsService.GetEventsByLeague(criteria).OrderBy(x => x.MatchName);
            return EventListToJson(@event, checkedAll, events);
        }
        #endregion Utilities
        
        #region private method
        /// <summary>
        /// Generate event list to JSON.
        /// </summary>
        /// <param name="event">Event selected list.</param>
        /// <param name="checkedAll">Checked All.</param>
        /// <param name="events">All Events.</param>
        /// <returns>Return JSONRESULT.</returns>
        private JsonResult EventListToJson(string @event, bool checkedAll, IOrderedEnumerable<EventEntity> events)
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
        private JsonResult LeagueListToJson(string selectedLeagues, bool checkedAll, IEnumerable<LeagueEntity> leagues)
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

        /// <summary>
        /// Generate Bet List.
        /// </summary>
        /// <param name="criteria">Bet List By Match Criteria.</param>
        /// <returns>Return JSONRESULT.</returns>
        private JsonResult BetList(BetListByMatchCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var tickets = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = tickets,
                IsAdmin = CurrentSession.IsAdmin
            };

            var betListTicketBuilder = new TotalbetsTicketBuilder(ticketBuilderParameters);

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = betListTicketBuilder.BuildTickets(),
                IsLastLevel = CurrentSession.AgentInfo.IsLastLevel,
                TotalItem = criteria.TotalRows
            });
        }
        #endregion
    }
}