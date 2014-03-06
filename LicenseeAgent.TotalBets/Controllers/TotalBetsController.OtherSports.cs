// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.OtherSports.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Utility;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public partial class TotalBetsController : BaseController
    {
        [HttpGet]
        public ActionResult OtherSports()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OtherSportsAllBetType(string matchName)
        {
            ViewBag.Match = matchName;
            return View();
        }

        /// <summary>
        /// Get All League.
        /// </summary>
        /// <param name="sportList">Sport List.</param>
        /// <param name="checkedAll">Sports are checked All.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetSportListOtherSports(string sportList, bool checkedAll)
        {
            var criteria = new OtherSportsSportListCriteria { AgentId = CurrentSession.AgentInfo.AgentId };

            var sports = _totalBetsService.GetOtherSportsList(criteria).OrderBy(x => x.SportName);
            var sportMultiItems = new List<dynamic>();
            var sportIds = string.IsNullOrEmpty(sportList) ? new List<string>() : sportList.Split(',').ToList();

            foreach (var item in sports)
            {
                sportMultiItems.Add(new
                {
                    id = item.SportType,
                    value = item.SportType,
                    text = item.SportName,
                    @checked = checkedAll || sportIds.Contains(item.SportType.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = sportMultiItems
            });
        }

        /// <summary>
        /// Get All League.
        /// </summary>
        /// <param name="sportList">Sport List.</param>
        /// <param name="leagueList">League list.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetLeagueListOtherSports(string sportList, string leagueList)
        {
            var criteria = new OtherSportsLeagueListCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportList = sportList };
            var leagues = _totalBetsService.GetOtherSportsLeagues(criteria).OrderBy(x => x.LeagueName);
            var leagueIds = string.IsNullOrEmpty(leagueList) ? new List<string>() : leagueList.Split(',').ToList();
            var leagueMultiItems = new List<dynamic>();
            foreach (var item in leagues)
            {
                leagueMultiItems.Add(new
                {
                    id = item.LeagueId,
                    value = item.LeagueId,
                    text = item.LeagueName,
                    @checked = string.IsNullOrEmpty(leagueList) || leagueIds.Contains(item.LeagueId.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = leagueMultiItems,
            });
        }

        /// <summary>
        /// Get events by leagues.
        /// </summary>
        /// <param name="sportList">Sport Type List.</param>
        /// <param name="leagueList">League value.</param>
        /// <param name="eventList">Event List value.</param>
        /// <returns>Events result.</returns>
        [HttpPost]
        public JsonResult GetEventsByLeaguesOtherSports(string sportList, string leagueList, string eventList)
        {
            var criteria = new OtherSportsEventListCriteria()
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                SportList = sportList,
                LeagueList = leagueList
            };

            var events = _totalBetsService.GetOtherSportsEventsByLeague(criteria).OrderBy(x => x.MatchName);
            var eventIds = string.IsNullOrEmpty(eventList) ? new List<string>() : eventList.Split(',').ToList();
            var eventMultiItems = new List<dynamic>();
            foreach (var item in events)
            {
                eventMultiItems.Add(new
                {
                    id = item.MatchId,
                    value = item.MatchId,
                    text = item.MatchName,
                    @checked = string.IsNullOrEmpty(eventList) || eventIds.Contains(item.MatchId.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = eventMultiItems,
            });
        }

        /// <summary>
        /// Get Other Sports Data.
        /// </summary>
        /// <param name="model">Other Sports View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetOtherSportsData(OtherSportsViewModel model)
        {
            if (ModelState.IsValid)
            {
                OtherSportsCriteria criteria = new OtherSportsCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    SportList = model.SportList,
                    LeagueList = model.LeagueList,
                    MatchList = model.MatchList
                };
                var dataList = _totalBetsService.GetOtherSportsData(criteria);
                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    List = MapTotalBetsOtherSportsData(dataList).ToList()
                });
            }

            return Json(new Result(Result.FailureValue));
        }

        /// <summary>
        /// Map Total Bets Other Sports Data.
        /// </summary>
        /// <param name="dataList">Data List from database.</param>
        /// <returns>List Sport Group Entity.</returns>
        private static IList<SportGroupEntity> MapTotalBetsOtherSportsData(IEnumerable<TotalBetsEntity> dataList)
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
    }
}