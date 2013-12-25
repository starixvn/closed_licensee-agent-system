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
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public class TotalBetsController : BaseController
    {
        private readonly ITotalBetsService _totalBetsService;

        public TotalBetsController(TotalBetsService totalBetsService)
        {
            _totalBetsService = totalBetsService;
        }

        [HttpGet]
        public ViewResult Soccer()
        {
            var model = new SoccerViewModel() { League = string.Empty, Event = string.Empty, TimeInterval = 15 };

            return GetSoccerData(model);
        }

        [HttpPost]
        public ViewResult Soccer(SoccerViewModel model)
        {
            return GetSoccerData(model);
        }

        [HttpGet]
        public ViewResult SoccerDetail(int matchId, string matchName)
        {
            var criteria = new SoccerDetailCriteria { AgentId = CurrentSession.AgentInfo.AgentId, MatchID = matchId };
            var dataList = _totalBetsService.GetSoccerDetailData(criteria);
            ViewBag.MatchName = matchName;
            var model = new SoccerDetailViewModel { MainData = dataList.ToList() };
            return View(model);
        }

        [NonAction]
        public ViewResult GetSoccerData(SoccerViewModel model)
        {
            var criteria = new SoccerCriteria { AgentId = CurrentSession.AgentInfo.AgentId, LeagueList = model.League, MatchList = model.Event };
            var dataList = _totalBetsService.GetSoccerData(criteria);
            model.MainData = GetMainData(dataList);

            return View(model);
        }

        #region Ultilities

        /// <summary>
        /// Get main display data to view.
        /// </summary>
        /// <param name="dataList">Data list value.</param>
        /// <returns>Group league list view data.</returns>
        [NonAction]
        public IList<LeagueGroupEntity> GetMainData(IEnumerable<SoccerDataEntity> dataList)
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
                var matchGroupList = matchsInLeague.Select(x => new { x.MatchId, x.IsLive, x.LiveHomeScore, x.LiveAwayScore, x.LiveIndicator }).Distinct().ToList();
                foreach (var matchGroup in matchGroupList)
                {
                    var matchs = matchsInLeague.Where(x => x.MatchId == matchGroup.MatchId && x.IsLive == matchGroup.IsLive &&
                        x.LiveHomeScore == matchGroup.LiveHomeScore && x.LiveAwayScore == matchGroup.LiveAwayScore && x.LiveIndicator == matchGroup.LiveIndicator).ToList();
                    var match = new MatchEntity
                    {
                        MatchId = matchs.FirstOrDefault().MatchId,
                        Index = index++,
                        Time = matchGroup.IsLive 
                            ? string.Format(timeLiveTemp, matchGroup.LiveHomeScore, matchGroup.LiveAwayScore) 
                            : string.Format(timeDeadBallTemp, matchs.First().KickOffTime),
                        HomeId = matchs.FirstOrDefault().HomeId,
                        AwayId = matchs.FirstOrDefault().AwayId,
                        HomeFullTimeHandicap = SetTotalBetsValues(matchs, 1, "h"),
                        AwayFullTimeHandicap = SetTotalBetsValues(matchs, 1, "a"),
                        HomeFullTimeOverUnder = SetTotalBetsValues(matchs, 3, "h"),
                        AwayFullTimeOverUnder = SetTotalBetsValues(matchs, 3, "a"),
                        HomeFullTimeOneXTwo = SetTotalBetsValues(matchs, 5, "1"),
                        AwayFullTimeOneXTwo = SetTotalBetsValues(matchs, 5, "2"),
                        DrawFullTimeOneXTwo = SetTotalBetsValues(matchs, 5, "x"),
                        HomeFirstHaflHandicap = SetTotalBetsValues(matchs, 7, "h"),
                        AwayFirstHaflHandicap = SetTotalBetsValues(matchs, 7, "a"),
                        HomeFirstHaflOverUnder = SetTotalBetsValues(matchs, 8, "h"),
                        AwayFirstHaflOverUnder = SetTotalBetsValues(matchs, 8, "a"),
                        HomeFirstHaflOneXTwo = SetTotalBetsValues(matchs, 15, "1"),
                        AwayFirstHaflOneXTwo = SetTotalBetsValues(matchs, 15, "2"),
                        DrawFirstHaflOneXTwo = SetTotalBetsValues(matchs, 15, "x"),
                    };
                    matchList.Add(match);
                }

                result.Add(new LeagueGroupEntity { LeagueId = leagueId, MatchList = matchList });
            }

            return result;
        }

        /// <summary>
        /// Set total bet value.
        /// </summary>
        /// <param name="matchs">Match list.</param>
        /// <param name="betType">Bet type value.</param>
        /// <param name="betTeam">Bet team value.</param>
        /// <returns>Total bet string.</returns>
        [NonAction]
        public string SetTotalBetsValues(IEnumerable<SoccerDataEntity> matchs, int betType, string betTeam)
        {
            var matchByBet = matchs.Where(x => x.BetType == betType && x.BetTeam == betTeam);
            if (matchByBet.Any())
            {
                var match = matchByBet.FirstOrDefault();
                var template = "({0}) {1}";
                var betCountString = Formatter.FormatInt(match.TotalTrans);
                var stakeString = match.Stake % 1 == 0 ? Formatter.FormatInt(match.Stake) : Formatter.FormatDecimal(match.Stake);

                return string.Format(template, betCountString, stakeString);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get All League.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <returns>League list.</returns>
        [HttpPost]
        public JsonResult GetLeagueList(string league)
        {
            var criteria = new SoccerLeagueCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportType = 1 };
            var leagues = _totalBetsService.GetLeagues(criteria);            
            var leagueIds = string.IsNullOrEmpty(league) ? new List<string>() : league.Split(',').ToList();
            var leagueMultiItems = new List<dynamic>();
            foreach (var item in leagues)
            {
                leagueMultiItems.Add(new
                {
                    id = item.LeagueId,
                    value = item.LeagueId,
                    text = item.LeagueName,
                    @checked = string.IsNullOrEmpty(league) || leagueIds.Contains(item.LeagueId.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                MsgId = Result.SuccessValue,
                List = leagueMultiItems,
            });
        }

        /// <summary>
        /// Get events by leagues.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <param name="event">Event value.</param>
        /// <returns>Events result.</returns>
        [HttpPost]
        public JsonResult GetEventsByLeagues(string league, string @event)
        {
            var criteria = new SoccerEventCriteria() { AgentId = CurrentSession.AgentInfo.AgentId, SportType = 1, LeagueList = league };
            var events = _totalBetsService.GetEventsByLeague(criteria);
            var eventIds = string.IsNullOrEmpty(@event) ? new List<string>() : @event.Split(',').ToList();
            var eventMultiItems = new List<dynamic>();
            foreach (var item in events)
            {
                eventMultiItems.Add(new
                {
                    id = item.MatchId,
                    value = item.MatchId,
                    text = item.MatchName,
                    @checked = string.IsNullOrEmpty(@event) || eventIds.Contains(item.MatchId.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                MsgId = Result.SuccessValue,
                List = eventMultiItems,
            });
        }

        #endregion Ultilities
    }
}