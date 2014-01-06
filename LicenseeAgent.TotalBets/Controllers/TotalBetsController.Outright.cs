// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.Outright.cs" company="Nexcel Solutions Vietnam">
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
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public partial class TotalBetsController : BaseController
    {
        [HttpGet]
        public ViewResult Outright()
        {
            var model = new OutrightViewModel() { TimeInterval = 15 };

            return View(model);
        }

        [HttpPost]
        public JsonResult GetOutrightData(OutrightCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var dataList = _totalBetsService.GetOutrightData(criteria);
            return Json(new
            {
                Result.SuccessValue,
                List = MapOutrightMainData(dataList)
            });
        }

        #region Ultilities

        /// <summary>
        /// Map outright main data.
        /// </summary>
        /// <param name="dataList">Data list value.</param>
        /// <returns>Outright data result.</returns>
        public IList<dynamic> MapOutrightMainData(IEnumerable<OutrightEntity> dataList)
        {
            IList<dynamic> sportItems = new List<dynamic>();
            var sports = dataList.Select(s => new { s.SportType, s.SportName }).Distinct().OrderBy(s => s.SportName);
            foreach (var sport in sports)
            {
                var allLeagueData = dataList.Where(le => le.SportType == sport.SportType);
                var leaguesInSport = allLeagueData.Select(l => new { l.LeagueId, l.LeagueName }).Distinct().OrderBy(l => l.LeagueName);
                var leagueItems = new List<dynamic>();
                foreach (var league in leaguesInSport)
                {
                    var teamsInLeague = allLeagueData.Where(t => t.LeagueId == league.LeagueId);
                    var leagueItem = new { LeagueId = league.LeagueId, LeagueName = league.LeagueName, TeamList = teamsInLeague };
                    leagueItems.Add(leagueItem);
                }

                var sportItem = new { SportType = sport.SportType, SportName = sport.SportName, LeagueList = leagueItems };
                sportItems.Add(sportItem);
            }

            return sportItems;
        }

        /// <summary>
        /// Get sport list.
        /// </summary>
        /// <param name="sport">Sport value.</param>
        /// <returns>Sport list.</returns>
        [HttpPost]
        public JsonResult GetSportList(string sport)
        {
            var criteria = new OutrightSportCriteria { AgentId = CurrentSession.AgentInfo.AgentId };
            var sports = _totalBetsService.GetOutrightSports(criteria).OrderBy(x => x.SportName);
            var sportIds = string.IsNullOrEmpty(sport) ? new List<string>() : sport.Split(',').ToList();
            var sportMultiItems = new List<dynamic>();
            foreach (var item in sports)
            {
                sportMultiItems.Add(new
                {
                    id = item.SportType,
                    value = item.SportType,
                    text = item.SportName,
                    @checked = string.IsNullOrEmpty(sport) || sportIds.Contains(item.SportType.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = sportMultiItems,
            });
        }

        /// <summary>
        /// Get Leagues by sports.
        /// </summary>
        /// <param name="sport">Sport value.</param>
        /// <param name="league">League Type.</param>
        /// <returns>League list.</returns>
        [HttpPost]
        public JsonResult GetLeaguesBySports(string sport, string league)
        {
            var criteria = new OutrightLeagueCriteria { AgentId = CurrentSession.AgentInfo.AgentId };
            var leagues = _totalBetsService.GetOutrightLeagues(criteria).OrderBy(x => x.LeagueName);
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
                ErrCode = Result.SuccessValue,
                List = leagueMultiItems,
            });
        }

        /// <summary>
        /// Get teams by leagues.
        /// </summary>
        /// <param name="league">League value.</param>
        /// <param name="team">Team Type.</param>
        /// <returns>Team list.</returns>
        [HttpPost]
        public JsonResult GetTeamsByLeagues(string league, string team)
        {
            var criteria = new OutrightTeamCriteria { AgentId = CurrentSession.AgentInfo.AgentId };
            var teams = _totalBetsService.GetOutrightTeam(criteria).OrderBy(x => x.TeamName);
            var teamIds = string.IsNullOrEmpty(league) ? new List<string>() : league.Split(',').ToList();
            var teamMultiItems = new List<dynamic>();
            foreach (var item in teams)
            {
                teamMultiItems.Add(new
                {
                    id = item.TeamId,
                    value = item.TeamId,
                    text = item.TeamName,
                    @checked = string.IsNullOrEmpty(team) || teamIds.Contains(item.TeamId.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = teamMultiItems,
            });
        }

        #endregion Ultilities
    }
}