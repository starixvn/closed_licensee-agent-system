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
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public partial class TotalBetsController : BaseController
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
            model.Event = model.Event ?? string.Empty;
            model.League = model.League ?? string.Empty;
            return GetSoccerData(model);
        }

        [HttpGet]
        public ViewResult SoccerDetail(int matchId, string matchName)
        {
            var criteria = new SoccerDetailCriteria { AgentId = CurrentSession.AgentInfo.AgentId, MatchId = matchId };
            var dataList = _totalBetsService.GetSoccerDetailData(criteria);
            var model = new SoccerDetailViewModel { MatchName = matchName, MainData = dataList.ToList() };
            return View(model);
        }

        [NonAction]
        public ViewResult GetSoccerData(SoccerViewModel model)
        {
            var criteria = new SoccerCriteria { AgentId = CurrentSession.AgentInfo.AgentId, LeagueList = model.League, MatchList = model.Event };
            var dataList = _totalBetsService.GetSoccerData(criteria).OrderByDescending(x => x.LiveIndicator);
            model.MainData = MapTotalBetsMainData(dataList).OrderBy(x => x.LeagueName).ToList();

            return View(model);
        }

        #region Ultilities

        /// <summary>
        /// Get main display data to view.
        /// </summary>
        /// <param name="dataList">Data list value.</param>
        /// <returns>Group league list view data.</returns>
        [NonAction]
        public IList<LeagueGroupEntity> MapTotalBetsMainData(IEnumerable<SoccerDataEntity> dataList)
        {
            var timeLiveTemplate = "{0}-{1}<br /><span class='live'>" + Las.Live + "!</span>";
            IList<LeagueGroupEntity> result = new List<LeagueGroupEntity>();
            var leagueIdList = dataList.Select(x => x.LeagueId).Distinct().ToList();
            foreach (var leagueId in leagueIdList)
            {
                var matchList = new List<MatchEntity>();
                var matchsInLeague = dataList.Where(x => x.LeagueId == leagueId).ToList();
                var matchGroupList = matchsInLeague.Select(x => new { x.MatchId, x.LiveHomeScore, x.LiveAwayScore, x.LiveIndicator }).Distinct().ToList();
                foreach (var matchGroup in matchGroupList)
                {
                    var matches = matchsInLeague.Where(x =>
                        x.MatchId == matchGroup.MatchId &&
                        x.LiveHomeScore == matchGroup.LiveHomeScore &&
                        x.LiveAwayScore == matchGroup.LiveAwayScore &&
                        x.LiveIndicator == matchGroup.LiveIndicator).ToList();
                    var match = new MatchEntity
                    {
                        MatchId = matches.FirstOrDefault().MatchId,
                        LiveIndicator = matchGroup.LiveIndicator,
                        Time = matchGroup.LiveIndicator
                            ? string.Format(timeLiveTemplate, matchGroup.LiveHomeScore, matchGroup.LiveAwayScore)
                            : matches.FirstOrDefault().ShowTime,
                        HomeId = matches.FirstOrDefault().HomeId,
                        AwayId = matches.FirstOrDefault().AwayId,
                        HomeFulltimeHandicap = SetTotalTransAndStakeValues(matches, 1, "h"),
                        AwayFulltimeHandicap = SetTotalTransAndStakeValues(matches, 1, "a"),
                        HomeFulltimeOverUnder = SetTotalTransAndStakeValues(matches, 3, "h"),
                        AwayFulltimeOverUnder = SetTotalTransAndStakeValues(matches, 3, "a"),
                        HomeFulltimeOneXTwo = SetTotalTransAndStakeValues(matches, 5, "1"),
                        AwayFulltimeOneXTwo = SetTotalTransAndStakeValues(matches, 5, "2"),
                        DrawFulltimeOneXTwo = SetTotalTransAndStakeValues(matches, 5, "x"),
                        HomeFirstHalfHandicap = SetTotalTransAndStakeValues(matches, 7, "h"),
                        AwayFirstHalfHandicap = SetTotalTransAndStakeValues(matches, 7, "a"),
                        HomeFirstHalfOverUnder = SetTotalTransAndStakeValues(matches, 8, "h"),
                        AwayFirstHalfOverUnder = SetTotalTransAndStakeValues(matches, 8, "a"),
                        HomeFirstHalfOneXTwo = SetTotalTransAndStakeValues(matches, 15, "1"),
                        AwayFirstHalfOneXTwo = SetTotalTransAndStakeValues(matches, 15, "2"),
                        DrawFirstHalfOneXTwo = SetTotalTransAndStakeValues(matches, 15, "x"),
                    };
                    matchList.Add(match);
                }

                result.Add(new LeagueGroupEntity { LeagueId = leagueId, MatchList = matchList });
            }

            return result;
        }

        /// <summary>
        /// Set total transaction and stake value.
        /// </summary>
        /// <param name="matchs">Match list.</param>
        /// <param name="betType">Bet type value.</param>
        /// <param name="betTeam">Bet team value.</param>
        /// <returns>Total bet string.</returns>
        [NonAction]
        public string SetTotalTransAndStakeValues(IEnumerable<SoccerDataEntity> matchs, int betType, string betTeam)
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