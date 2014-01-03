// -----------------------------------------------------------------------
// <copyright file="TotalBetsSoccerController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Resources;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.Utils;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public partial class TotalBetsController : BaseController
    {
        private readonly ITotalBetsService _totalBetsService;

        public TotalBetsController(TotalBetsService totalBetsService)
        {
            _totalBetsService = totalBetsService;
        }

        [HttpGet]
        public ViewResult Soccer()
        {
            var model = new SoccerViewModel() { League = string.Empty, Event = string.Empty, TimeInterval = 15 };

            return View(model);
        }

        [HttpGet]
        public ViewResult SoccerDetail(int matchId, string matchName)
        {
            var criteria = new SoccerDetailCriteria { AgentId = CurrentSession.AgentInfo.AgentId, MatchId = matchId };
            var dataList = _totalBetsService.GetSoccerDetailData(criteria);
            var model = new SoccerDetailViewModel { MatchName = matchName, MainData = dataList.ToList() };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetSoccerData(SoccerCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var dataList = _totalBetsService.GetSoccerData(criteria);
            return Json(new
            {
                List = TotalBetsUtils.MapTotalBetsMainData(dataList).OrderByDescending(x => x.LiveIndicator).ThenBy(x => x.LeagueName).ToList()
            });
        }

        /// <summary>
        /// Get All League for Soccer and Basketball.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <param name="sportType">Sport Type.</param>
        /// <returns>League list.</returns>
        [HttpPost]
        public JsonResult GetLeagueList(string league, int sportType = 1)
        {
            var criteria = new LeagueCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportType = sportType };
            var leagues = _totalBetsService.GetLeagues(criteria).OrderBy(x => x.LeagueName);
            var leagueIds = string.IsNullOrEmpty(league) ? new List<string>() : league.Split(',').ToList();
            var leagueMultiItems = new List<dynamic>();
            foreach (var item in leagues)
            {
                leagueMultiItems.Add(new
                {
                    id = item.LeagueId,
                    value = item.LeagueId,
                    text = item.LeagueName,
                    @checked = string.IsNullOrEmpty(league) || leagueIds.Contains(item.LeagueId.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                MsgId = Result.SuccessValue,
                List = leagueMultiItems,
            });
        }

        /// <summary>
        /// Get events for Soccer and Basketball by leagues.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <param name="event">Event value.</param>
        /// <param name="sportType">Sport Type.</param>
        /// <returns>Events result.</returns>
        [HttpPost]
        public JsonResult GetEventsByLeagues(string league, string @event, int sportType = 1)
        {
            var criteria = new EventCriteria() { AgentId = CurrentSession.AgentInfo.AgentId, SportType = sportType, LeagueList = league };
            var events = _totalBetsService.GetEventsByLeague(criteria).OrderBy(x => x.MatchName);
            var eventIds = string.IsNullOrEmpty(@event) ? new List<string>() : @event.Split(',').ToList();
            var eventMultiItems = new List<dynamic>();
            foreach (var item in events)
            {
                eventMultiItems.Add(new
                {
                    id = item.MatchId,
                    value = item.MatchId,
                    text = item.MatchName,
                    @checked = string.IsNullOrEmpty(@event) || eventIds.Contains(item.MatchId.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                MsgId = Result.SuccessValue,
                List = eventMultiItems,
            });
        }
    }
}