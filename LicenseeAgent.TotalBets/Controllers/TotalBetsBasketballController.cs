// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Resources;
    using LicenseeAgent.TotalBets.Criterias;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.TotalBets.Entities;

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
                List = GenerateBasketballData(dataList)
            });
        }

        [NonAction]
        public IList<LeagueGroupEntity> GenerateBasketballData(IEnumerable<TotalBetsEntity> dataList)
        {
            var timeLiveTemp = "{0}-{1}<br /><span class='live'>" + Las.Live + "!</span>";
            var timeDeadBallTemp = "<span class='live'>" + Las.Live + "!</span><br />{hh:mmtt}";
            var eventTemp = "{0}<br /><br />{1}<br /><br />" + "<span class='draw'>" + Las.Draw + "</span>";
            var index = 0;
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
                        Index = index++,
                        Time = totalBet.ShowTime,
                        HomeId = totalBet.HomeId,
                        AwayId = totalBet.AwayId,
                        HomeFullTimeHandicap = SetTotalBetsValues(matchs, 1, "h"),
                        AwayFullTimeHandicap = SetTotalBetsValues(matchs, 1, "a"),
                        HomeFullTimeOddEven = SetTotalBetsValues(matchs, 2, "h"),
                        AwayFullTimeOddEven = SetTotalBetsValues(matchs, 2, "a"),
                        HomeFullTimeOverUnder = SetTotalBetsValues(matchs, 3, "h"),
                        AwayFullTimeOverUnder = SetTotalBetsValues(matchs, 3, "a"),
                        HomeFirstHaflHandicap = SetTotalBetsValues(matchs, 7, "h"),
                        AwayFirstHaflHandicap = SetTotalBetsValues(matchs, 7, "a"),
                        HomeFirstHaflOverUnder = SetTotalBetsValues(matchs, 8, "h"),
                        AwayFirstHaflOverUnder = SetTotalBetsValues(matchs, 8, "a"),
                        HomeFirstHaflOddEven = SetTotalBetsValues(matchs, 12, "h"),
                        AwayFirstHaflOddEven = SetTotalBetsValues(matchs, 12, "a")
                    };
                    matchList.Add(match);
                }

                result.Add(new LeagueGroupEntity { LeagueId = leagueId, MatchList = matchList });
            }

            return result;
        }
    }
}