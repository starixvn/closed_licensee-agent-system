// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.Soccer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;
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
            var betTypeTableItems = new List<BetTypeTableItem>();
            betTypeTableItems.Add(new BetTypeTableItem
            {
                BetType = (int)BetType.BetType_2,
                NumberOfColumn = 1,
                Rows = new BetTypeRowItem[]
                {
                    new BetTypeRowItem(Las.Odd, "h"),
                    new BetTypeRowItem(Las.Even, "a")
                }
            });

            var specialChars = new char[] { ' ', '\'', '/', '.', '-' };
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

        #region Utilities

        /// <summary>
        /// Get All League.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <param name="sportType">Sport Type.</param>
        /// <returns>League list.</returns>
        [HttpPost]
        public JsonResult GetLeagueList(string league, int sportType)
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
                ErrCode = Result.SuccessValue,
                List = leagueMultiItems,
            });
        }

        /// <summary>
        /// Get events by leagues.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <param name="event">Event value.</param>
        /// <param name="sportType">Sport Type.</param>
        /// <returns>Events result.</returns>
        [HttpPost]
        public JsonResult GetEventsByLeagues(string league, string @event, int sportType)
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
                ErrCode = Result.SuccessValue,
                List = eventMultiItems,
            });
        }

        #endregion Utilities
    }
}