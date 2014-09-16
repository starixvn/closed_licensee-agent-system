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
    /// Account controllers.
    /// </summary>
    public class BasketballController : CommonController
    {
        #region Variables

        private readonly IBasketballService _basketballService;

        #endregion Variables

        #region Constructors

        public BasketballController()
            : this(null)
        {
        }

        public BasketballController(BasketballService basketballService)
        {
            _basketballService = basketballService ?? new BasketballService();
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
        /// Get Basketball Data.
        /// </summary>
        /// <param name="model">Total Bets View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult Index(TotalBetsViewModel model)
        {
            var criteria = new BasketballCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                LeagueList = model.LeagueList,
                MatchList = model.MatchList
            };
            var dataList = _basketballService.GetBasketballData(criteria);
            var orderedDataList = TotalBetsUtility.MapTotalBetsMainData(dataList)
                .OrderByDescending(x => x.LiveIndicator)
                .ThenBy(x => x.LeagueName)
                .ToList();

            return Json(orderedDataList);
        }

        #endregion Index

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
    }
}