namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.TotalBets.Core;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Outright controllers.
    /// </summary>
    public class OutrightController : TotalBetsController
    {
        #region Variables

        private readonly IOutrightService _outrightService;

        #endregion Variables

        #region Constructors

        public OutrightController()
            : this(null)
        {
        }

        public OutrightController(OutrightService outrightService)
        {
            _outrightService = outrightService ?? new OutrightService();
        }

        #endregion Constructors

        #region Index

        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.PageContext = new PageContext(CurrentSession, 0);
            var model = new OutrightViewModel
            {
                TimeInterval = Constant.RefreshCountDownInterval
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult Index(OutrightCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var dataList = _outrightService.GetOutrightData(criteria);
            return Json(Map(dataList));
        }

        /// <summary>
        /// Map outright main data.
        /// </summary>
        /// <param name="dataList">Data list value.</param>
        /// <returns>Outright data result.</returns>
        [NonAction]
        public IList<dynamic> Map(IList<OutrightEntity> dataList)
        {
            var sportItems = new List<dynamic>();
            var sports = dataList.Select(s => new { s.SportType, s.SportName }).Distinct().OrderBy(s => s.SportName).ToList();
            foreach (var sport in sports)
            {
                var allLeagueData = dataList.Where(l => l.SportType == sport.SportType).ToList();
                var leaguesInSport = allLeagueData.Select(l => new { l.LeagueId, l.LeagueName }).Distinct().OrderBy(l => l.LeagueName).ToList();
                var leagueItems = new List<dynamic>();
                foreach (var league in leaguesInSport)
                {
                    var teamsInLeague = allLeagueData.Where(t => t.LeagueId == league.LeagueId).ToList();
                    var leagueItem = new { LeagueId = league.LeagueId, LeagueName = league.LeagueName, TeamList = teamsInLeague };
                    leagueItems.Add(leagueItem);
                }

                var sportItem = new { SportType = sport.SportType, SportName = sport.SportName, LeagueList = leagueItems };
                sportItems.Add(sportItem);
            }

            return sportItems;
        }

        #endregion Index

        #region Get sports

        /// <summary>
        /// Get sport list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Sport list.
        /// </returns>
        [HttpPost]
        public JsonResult GetSports(GetSportsViewModel model)
        {
            var criteria = new OutrightSportCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId
            };
            var sports = _outrightService.GetOutrightSports(criteria).OrderBy(x => x.SportName);
            var sportIds = string.IsNullOrEmpty(model.SelectedSports) ? new List<string>() : model.SelectedSports.Split(',').ToList();
            var sportMultiItems = sports.Select(x => new
            {
                id = x.SportType,
                value = x.SportType,
                text = x.SportName,
                @checked = model.CheckedAll || sportIds.Contains(x.SportType.ToString()),
                enable = true
            });

            return Json(sportMultiItems);
        }

        #endregion Get sports

        #region Get leagues

        /// <summary>
        /// Get Leagues by sports.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// League list.
        /// </returns>
        [HttpPost]
        public JsonResult GetLeagues(GetOtherSportsLeaguesViewModel model)
        {
            var criteria = new OutrightLeagueCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                SportList = model.SelectedSports
            };
            var leagues = _outrightService.GetOutrightLeagues(criteria).OrderBy(x => x.LeagueName).ToList();
            return GetLeagues(leagues, model.SelectedLeagues, model.CheckedAll);
        }

        #endregion Get leagues

        #region Get teams

        /// <summary>
        /// Get teams by leagues.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Team list.
        /// </returns>
        [HttpPost]
        public JsonResult GetTeams(GetTeamViewModel model)
        {
            var criteria = new OutrightTeamCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                SportList = model.SelectedSports,
                LeagueList = model.SelectedLeagues
            };
            var teams = _outrightService.GetOutrightTeam(criteria).OrderBy(x => x.TeamName).ToList();
            var teamIds = string.IsNullOrEmpty(model.SelectedTeams) ? new List<string>() : model.SelectedTeams.Split(',').ToList();
            var teamMultiItems = teams.Select(x => new
            {
                id = x.TeamId,
                value = x.TeamId,
                text = x.TeamName,
                @checked = model.CheckedAll || teamIds.Contains(x.TeamId.ToString()),
                enable = true
            });

            return Json(teamMultiItems);
        }

        #endregion Get teams
    }
}