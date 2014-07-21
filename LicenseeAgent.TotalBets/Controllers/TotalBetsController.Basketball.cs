namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Utility;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public partial class TotalBetsController
    {
        [HttpGet]
        public ActionResult Basketball()
        {
            ViewBag.PageContext = new PageContext(CurrentSession, 0);
            return View();
        }

        [HttpGet]
        public ActionResult BasketballAllBetType(string matchName)
        {
            ViewBag.Match = matchName;
            return View();
        }

        /// <summary>
        /// Get Basketball Data.
        /// </summary>
        /// <param name="model">Total Bets View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetBasketballData(TotalBetsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new BasketballCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    LeagueList = model.LeagueList,
                    MatchList = model.MatchList
                };
                var dataList = _totalBetsService.GetBasketballData(criteria);
                var orderedDataList = TotalBetsUtility.MapTotalBetsMainData(dataList)
                    .OrderByDescending(x => x.LiveIndicator)
                    .ThenBy(x => x.LeagueName)
                    .ToList();

                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    List = orderedDataList
                });
            }

            return Json(new Result(Result.FailureValue));
        }

        /// <summary>
        /// Gets the basketball bet list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns JSON object.</returns>
        [HttpPost]
        public JsonResult GetBasketballBetList(BetListByMatchCriteria criteria)
        {
            return BetList(criteria);
        }
    }
}