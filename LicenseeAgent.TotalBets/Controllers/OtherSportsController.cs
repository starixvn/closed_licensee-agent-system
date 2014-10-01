namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.Utility;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public class OtherSportsController : TotalBetsController
    {
        #region Variables

        private readonly IOtherSportsService _otherSportsService;

        #endregion Variables

        #region Constructors

        public OtherSportsController()
            : this(null)
        {
        }

        public OtherSportsController(OtherSportsService otherSportsService)
        {
            _otherSportsService = otherSportsService ?? new OtherSportsService();
        }

        #endregion Constructors

        #region Index

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PageContext = new PageContext(CurrentSession, 0);
            return View();
        }

        /// <summary>
        /// Get Other Sports Data.
        /// </summary>
        /// <param name="model">Other Sports View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult Index(OtherSportsViewModel model)
        {
            var criteria = new OtherSportsCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                SportList = model.SportList,
                LeagueList = model.LeagueList,
                MatchList = model.MatchList
            };
            var dataList = _otherSportsService.GetOtherSportsData(criteria);
            return Json(Map(dataList));
        }

        /// <summary>
        /// Map Total Bets Other Sports Data.
        /// </summary>
        /// <param name="dataList">Data List from database.</param>
        /// <returns>List Sport Group Entity.</returns>
        [NonAction]
        public IList<SportGroupEntity> Map(IList<TotalBetsEntity> dataList)
        {
            var liveEventsResult = new List<SportGroupEntity>();
            var deadEventsResult = new List<SportGroupEntity>();

            // Order by LiveIndicator.
            var sportIdList = dataList.OrderByDescending(m => m.LiveIndicator).Select(x => x.SportType).Distinct().ToList();
            foreach (var sportId in sportIdList)
            {
                var leaguesInSportLive = dataList.Where(m => m.SportType == sportId && m.LiveIndicator == true).ToList();
                var leaguesInSportDead = dataList.Where(m => m.SportType == sportId && m.LiveIndicator == false).ToList();
                if (leaguesInSportLive.Any())
                {
                    liveEventsResult.Add(new SportGroupEntity
                    {
                        SportType = sportId,
                        LeagueList = TotalBetsUtility.MapTotalBetsMainData(leaguesInSportLive).ToList()
                    });
                }

                if (leaguesInSportDead.Any())
                {
                    deadEventsResult.Add(new SportGroupEntity
                    {
                        SportType = sportId,
                        LeagueList = TotalBetsUtility.MapTotalBetsMainData(leaguesInSportDead).ToList()
                    });
                }
            }

            // Add dead match to the end.
            liveEventsResult.AddRange(deadEventsResult);
            return liveEventsResult;
        }

        #endregion Index

        #region Get sports

        /// <summary>
        /// Get All League.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JSON Result.
        /// </returns>
        [HttpPost]
        public JsonResult GetSports(GetSportsViewModel model)
        {
            var criteria = new OtherSportsSportListCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId
            };
            var sports = _otherSportsService.GetOtherSports(criteria).OrderBy(x => x.SportName);
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
        /// Get All League.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JSON Result.
        /// </returns>
        [HttpPost]
        public JsonResult GetLeagues(GetOtherSportsLeaguesViewModel model)
        {
            var criteria = new OtherSportsLeagueListCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                SportList = model.SelectedSports
            };
            var leagues = _otherSportsService.GetOtherSportsLeagues(criteria).OrderBy(x => x.LeagueName).ToList();
            return GetLeagues(leagues, model.SelectedLeagues, model.CheckedAll);
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
        [HttpPost]
        public JsonResult GetEvents(GetOtherSportsEventsViewModel model)
        {
            var criteria = new OtherSportsEventListCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                SportList = model.SelectedSports,
                LeagueList = model.SelectedLeagues
            };

            var events = _otherSportsService.GetOtherSportsEventsByLeague(criteria).OrderBy(x => x.MatchName).ToList();
            return GetEvents(events, model.SelectedEvents, model.CheckedAll);
        }

        #endregion Get events
    }
}