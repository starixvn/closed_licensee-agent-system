// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.Basketball.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Utils;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public partial class TotalBetsController : BaseController
    {
        [HttpGet]
        public ActionResult Basketball()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BasketballAllBetType(int matchId, string matchName)
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
                BasketballCriteria criteria = new BasketballCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    LeagueList = model.LeagueList,
                    MatchList = model.MatchList
                };
                var dataList = _totalBetsService.GetBasketballData(criteria);
                return Json(new
                {
                    MsgId = Result.SuccessValue,
                    List = TotalBetsUtils.MapTotalBetsMainData(dataList).OrderBy(x => x.LeagueName).ToList()
                });
            }

            return Json(new Result(Result.FailureValue));
        }
    }
}