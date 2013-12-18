// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.cs" company="Nexcel Solutions Vietnam">
// TotalBetsController.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Report.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Report.Criterias;
    using LicenseeAgent.Report.Services;
    using LicenseeAgent.Report.ViewModels;
    using LicenseeAgent.Web.Controllers;
    using System.Collections;
    using LicenseeAgent.Report.Entities;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TotalBetsController : BaseController
    {
        private ISoccerService _soccerService;

        public TotalBetsController(SoccerService soccerService)
        {
            _soccerService = soccerService;
        }

        [HttpGet]
        public ActionResult Soccer()
        {
            var criteria = new SoccerLeagueCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportType = 1 };
            //var leagues = _soccerService.GetLeagues(criteria);

            IList<LeagueEntity> leagues = new List<LeagueEntity>();
            leagues.Add(new LeagueEntity { LeagueId = 1 });
            leagues.Add(new LeagueEntity { LeagueId = 2 });
            leagues.Add(new LeagueEntity { LeagueId = 3 });
            leagues.Add(new LeagueEntity { LeagueId = 4 });

            ViewBag.Leagues = Json(new
            {
                MsgId = Result.SuccessValue,
                List = leagues
            });

            var model = new SoccerViewModel();

            return View(model);
        }
    }
}