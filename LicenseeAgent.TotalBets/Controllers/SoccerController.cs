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
    public partial class SoccerController : CommonController
    {
        private readonly ISoccerService _soccerService;

        public SoccerController()
            : this(null)
        {
        }

        public SoccerController(SoccerService soccerService)
        {
            _soccerService = soccerService ?? new SoccerService();
        }

        [HttpGet]
        public ViewResult Index(SoccerViewModel model)
        {
            ViewBag.PageContext = new PageContext(CurrentSession, 0);
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
            var dataList = _soccerService.GetSoccerDetailData(criteria);
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
        public ViewResult SoccerBetList(SoccerBetListViewModel model)
        {
            ViewBag.PageContext = new PageContext(CurrentSession);
            return View(model);
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
    }
}