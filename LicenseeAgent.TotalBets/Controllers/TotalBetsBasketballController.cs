// -----------------------------------------------------------------------
// <copyright file="TotalBetsBasketballController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;
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

        [HttpPost]
        public JsonResult GetBasketballData(BasketballCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;

            var dataList = _totalBetsService.GetBasketballData(criteria);

            return Json(new
            {
                List = GenerateBasketballData(dataList).OrderBy(x => x.LeagueName).ToList()
            });
        }

        /// <summary>
        /// Generate Basketball Data.
        /// </summary>
        /// <param name="dataList">List total bets entity.</param>
        /// <returns>List League Group Entity.</returns>
        [NonAction]
        public IList<LeagueGroupEntity> GenerateBasketballData(IEnumerable<TotalBetsEntity> dataList)
        {
            var timeLiveTemp = "{0}-{1}<br /><span class='live'>" + Las.Live + "!</span>";
            var timeDeadBallTemp = "<span class='live'>" + Las.Live + "!</span><br />{hh:mmtt}";
            var eventTemp = "{0}<br /><br />{1}<br /><br />" + "<span class='draw'>" + Las.Draw + "</span>";
            IList<LeagueGroupEntity> result = new List<LeagueGroupEntity>();
            var leagueIdList = dataList.Select(x => x.LeagueId).Distinct().ToList();
            foreach (var leagueId in leagueIdList)
            {
                var matchList = new List<MatchEntity>();
                var matchsInLeague = dataList.Where(x => x.LeagueId == leagueId).ToList();
                var matchGroupList = matchsInLeague.Select(x => new { x.MatchId, x.LiveHomeScore, x.LiveAwayScore, x.LiveIndicator }).Distinct().ToList();

                foreach (var matchGroup in matchGroupList)
                {
                    var matchs = matchsInLeague.Where(x => x.MatchId == matchGroup.MatchId &&
                        x.LiveHomeScore == matchGroup.LiveHomeScore && x.LiveAwayScore == matchGroup.LiveAwayScore && x.LiveIndicator == matchGroup.LiveIndicator).ToList();
                    var totalBet = matchs.FirstOrDefault();
                    var match = new MatchEntity
                    {
                        MatchId = totalBet.MatchId,
                        LiveIndicator = totalBet.LiveIndicator,
                        Time = totalBet.ShowTime,
                        HomeId = totalBet.HomeId,
                        AwayId = totalBet.AwayId,
                        HomeFulltimeHandicap = SetTotalBetsValues(matchs, 1, "h"),
                        AwayFulltimeHandicap = SetTotalBetsValues(matchs, 1, "a"),
                        HomeFulltimeOddEven = SetTotalBetsValues(matchs, 2, "h"),
                        AwayFulltimeOddEven = SetTotalBetsValues(matchs, 2, "a"),
                        HomeFulltimeOverUnder = SetTotalBetsValues(matchs, 3, "h"),
                        AwayFulltimeOverUnder = SetTotalBetsValues(matchs, 3, "a"),
                        HomeFirstHalfHandicap = SetTotalBetsValues(matchs, 7, "h"),
                        AwayFirstHalfHandicap = SetTotalBetsValues(matchs, 7, "a"),
                        HomeFirstHalfOverUnder = SetTotalBetsValues(matchs, 8, "h"),
                        AwayFirstHalfOverUnder = SetTotalBetsValues(matchs, 8, "a"),
                        HomeFirstHalfOddEven = SetTotalBetsValues(matchs, 12, "h"),
                        AwayFirstHalfOddEven = SetTotalBetsValues(matchs, 12, "a")
                    };
                    matchList.Add(match);
                }

                result.Add(new LeagueGroupEntity { LeagueId = leagueId, MatchList = matchList.OrderByDescending(m => m.LiveIndicator).ThenBy(n => n.MatchId).ToList() });
            }

            return result;
        }
    }
}