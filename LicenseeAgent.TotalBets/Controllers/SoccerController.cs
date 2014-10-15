namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.Utility;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Soccer controllers.
    /// </summary>
    public class SoccerController : TotalBetsController
    {
        #region Variables

        private readonly ISoccerService _soccerService;

        #endregion Variables

        #region Constructors

        public SoccerController()
            : this(null)
        {
        }

        public SoccerController(SoccerService soccerService)
        {
            _soccerService = soccerService ?? new SoccerService();
        }

        #endregion Constructors

        #region Index page

        [HttpGet]
        public ViewResult Index(SoccerViewModel model)
        {
            ViewBag.PageContext = new PageContext(CurrentSession, 0);
            return View(model);
        }

        /// <summary>
        /// Get soccer data.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Soccer data.</returns>
        [HttpPost]
        public JsonResult Index(SoccerCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var dataList = _soccerService.GetSoccerData(criteria);
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

            return Json(mapData);
        }

        #endregion Index page

        #region Get sports

        [HttpPost]
        public new JsonResult GetLeagues(GetSportLeaguesViewModel model)
        {
            return base.GetLeagues(model);
        }

        #endregion Get sports

        #region Get events

        [HttpPost]
        public new JsonResult GetEvents(GetSportEventsViewModel model)
        {
            return base.GetEvents(model);
        }

        #endregion Get events

        #region Detail

        /// <summary>
        /// Soccer the detail.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Detail of soccer page.</returns>
        [HttpGet]
        public ViewResult Detail(SoccerDetailViewModel model)
        {
            var criteria = new SoccerDetailCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                MatchId = model.MatchId
            };
            var dataList = _soccerService.GetSoccerDetailData(criteria);
            model.MainData = dataList.ToList();
            return View(model);
        }

        [HttpGet]
        public ViewResult DetailBetList(SoccerBetListViewModel model)
        {
            ViewBag.PageContext = new PageContext(CurrentSession);
            return View(model);
        }

        #endregion Detail
    }
}