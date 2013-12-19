// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Report.Criterias;
    using LicenseeAgent.Report.Entities;
    using LicenseeAgent.Report.Services;
    using LicenseeAgent.Report.ViewModels;
    using LicenseeAgent.TotalBets.Services;
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
            ViewBag.Leagues = GetLeagues();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetLeagues()
        {
            var criteria = new SoccerLeagueCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportType = 1 };
            //var leagues = _soccerService.GetLeagues(criteria);

            IList<LeagueEntity> leagues = new List<LeagueEntity>();
            leagues.Add(new LeagueEntity { LeagueId = 5 });
            leagues.Add(new LeagueEntity { LeagueId = 6 });
            leagues.Add(new LeagueEntity { LeagueId = 3 });
            leagues.Add(new LeagueEntity { LeagueId = 4 });

            return Json(new
            {
                MsgId = Result.SuccessValue,
                List = leagues
            }, JsonRequestBehavior.AllowGet);
        }
    }
}