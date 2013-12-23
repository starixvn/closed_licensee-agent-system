// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.TotalBets.Criterias;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public class TotalBetsController : BaseController
    {
        private readonly ITotalBetsService _totalBetsService;

        public TotalBetsController(TotalBetsService totalBetsService)
        {
            _totalBetsService = totalBetsService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Soccer()
        {
            var model = new SoccerViewModel() { League = string.Empty, Event = string.Empty };

            return GetSoccerData(model);
        }
                
        [HttpPost]
        public ActionResult Soccer(SoccerViewModel model)
        {
            return GetSoccerData(model);
        }

        [NonAction]
        public ActionResult GetSoccerData(SoccerViewModel model)
        {
            ////var criteria = new SoccerCriteria { AgentId = CurrentSession.AgentInfo.AgentId, LeagueList = model.League, MatchList = model.Event };

            var criteria = new SoccerCriteria { AgentId = 1000000111, LeagueList = string.Empty, MatchList = string.Empty };
            var dataList = _totalBetsService.GetSoccerData(criteria);

            return View(model);
        }

        #region Ultilities

        /// <summary>
        /// Get main display data to view.
        /// </summary>
        /// <param name="dataList">Data list value.</param>
        /// <returns>Group league list view data.</returns>
        [NonAction]
        public IList<LeagueGroupEntity> GetMainData(IEnumerable<SoccerDataEntity> dataList)
        {
            IList<LeagueGroupEntity> result = new List<LeagueGroupEntity>();

            return result;
        }

        /// <summary>
        /// Get All League.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <returns>League list.</returns>
        [HttpPost]
        public JsonResult GetLeagueList(string league)
        {
            var criteria = new SoccerLeagueCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportType = 1 };

            ////var leagues = _totalBetsService.GetLeagues(criteria);

            IList<LeagueEntity> leagues = new List<LeagueEntity>();
            leagues.Add(new LeagueEntity { LeagueId = 5 });
            leagues.Add(new LeagueEntity { LeagueId = 6 });
            leagues.Add(new LeagueEntity { LeagueId = 3 });
            leagues.Add(new LeagueEntity { LeagueId = 4 });

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
        /// Get events by leagues.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <param name="event">Event value.</param>
        /// <returns>Events result.</returns>
        [HttpPost]
        public JsonResult GetEventsByLeagues(string league, string @event)
        {
            var criteria = new SoccerEventCriteria() { AgentId = CurrentSession.AgentInfo.AgentId, SportType = 1, LeagueList = league };
            ////var events = _totalBetsService.GetEventsByLeague(criteria);

            IList<EventEntity> events = new List<EventEntity>();
            events.Add(new EventEntity { HomeTeam = 5, AwayTeam = 6, LeagueId = 4, IsLive = true, MatchId = 1 });
            events.Add(new EventEntity { HomeTeam = 7, AwayTeam = 10, LeagueId = 3, IsLive = true, MatchId = 2 });
            events.Add(new EventEntity { HomeTeam = 8, AwayTeam = 11, LeagueId = 5, IsLive = false, MatchId = 3 });
            events.Add(new EventEntity { HomeTeam = 9, AwayTeam = 12, LeagueId = 6, IsLive = true, MatchId = 4 });

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

        #endregion Ultilities
    }
}