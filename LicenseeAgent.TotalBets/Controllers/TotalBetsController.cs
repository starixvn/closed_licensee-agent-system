// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.TotalBets.Criterias;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public class TotalBetsController : BaseController
    {
        private readonly ITotalBetsService _totalBetsService;

        private readonly ISoccerService _soccerService;

        public TotalBetsController(TotalBetsService totalBetsService, SoccerService soccerService)
        {
            _totalBetsService = totalBetsService;
            _soccerService = soccerService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Soccer()
        {
            var model = new SoccerViewModel();

            return View(model);
        }

        /// <summary>
        /// Get All League.
        /// </summary>
        /// <returns>League list.</returns>
        [HttpPost]
        public JsonResult GetLeagues()
        {
            var criteria = new SoccerLeagueCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportType = 1 };

            var leagues = _soccerService.GetLeagues(criteria);

            //IList<LeagueEntity> leagues = new List<LeagueEntity>();
            //leagues.Add(new LeagueEntity { LeagueId = 5 });
            //leagues.Add(new LeagueEntity { LeagueId = 6 });
            //leagues.Add(new LeagueEntity { LeagueId = 3 });
            //leagues.Add(new LeagueEntity { LeagueId = 4 });

            return Json(new
            {
                MsgId = Result.SuccessValue,
                List = leagues
            });
        }

        [HttpPost]
        public JsonResult GetEventsByLeagues(string leagues)
        {
            var criteria = new SoccerEventCriteria() { AgentId = CurrentSession.AgentInfo.AgentId, SportType = 1, LeagueList = leagues };
            var events = _soccerService.GetEventsByLeague(criteria);

            //IList<EventEntity> events = new List<EventEntity>();
            //events.Add(new EventEntity { HomeTeam = 5, AwayTeam = 6, LeagueId = 4, IsLive = true, MatchId = 1 });
            //events.Add(new EventEntity { HomeTeam = 7, AwayTeam = 10, LeagueId = 3, IsLive = true, MatchId = 2 });
            //events.Add(new EventEntity { HomeTeam = 8, AwayTeam = 11, LeagueId = 5, IsLive = false, MatchId = 3 });
            //events.Add(new EventEntity { HomeTeam = 9, AwayTeam = 12, LeagueId = 6, IsLive = true, MatchId = 4 });

            return Json(new
            {
                MsgId = Result.SuccessValue,
                List = events
            });
        }
    }
}