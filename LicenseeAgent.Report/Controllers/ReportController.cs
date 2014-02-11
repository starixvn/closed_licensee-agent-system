// ----------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/14">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Core.Entities;
    using Core.Services;
    using Criteria;
    using Fanex.Result.Core;
    using Fanex.Result.Core.DTO;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Web;
    using Services;
    using ViewModels;
    using Web.Controllers;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class ReportController : BaseController
    {
        #region Constant

        private bool _includeTestData = false;

        #endregion Constant

        #region Services

        private ICoreService _coreService;

        private IReportService _reportService;

        #endregion Services

        #region Constructor

        public ReportController(ICoreService coreService, IReportService reportService)
        {
            _coreService = coreService;
            _reportService = reportService;
        }

        #endregion Constructor

        #region Results Report

        /// <summary>
        /// Index page of Results report.
        /// </summary>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Results()
        {
            return View();
        }

        /// <summary>
        /// Index page of Results report.
        /// </summary>
        /// <param name="criteria">Sport criteria.</param>
        /// <returns>Return a view.</returns>
        [HttpPost]
        public JsonResult Results(SportCriteria criteria)
        {
            criteria.IsShowAll = _includeTestData;
            var sports = _reportService.GetSports(criteria);
            var model = new ResultsReportViewModel();
            model.Sports = sports.Select(i => new SelectListItem { Text = i.SportName, Value = i.SportType }).OrderBy(j => j.Text).ToList();

            if (model.Sports.Count > 0)
            {
                model.Sports[0].Selected = true;
                var leagueCriteria = new LeagueCriteria()
                {
                    IsShowAll = criteria.IsShowAll,
                    EventDate = criteria.EventDate,
                    SportType = int.Parse(model.Sports[0].Value)
                };

                var leagues = _reportService.GetLeagues(leagueCriteria);
                model.Leagues = leagues.Select(i => new SelectListItem { Text = i.LeagueName, Value = i.LeagueID }).OrderBy(j => j.Text).ToList();

                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    Sports = model.Sports,
                    Leagues = model.Leagues,
                    Results = GetMatchResultsData(leagueCriteria, 0)
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Sports = new List<SelectListItem>(),
                Leagues = new List<SelectListItem>(),
                Results = ResultsMatchNoData()
            });
        }

        /// <summary>
        /// Index page of Results report.
        /// </summary>
        /// <param name="criteria">Sport criteria.</param>
        /// <returns>Return a view.</returns>
        [HttpPost]
        public JsonResult OutrightResults(OutrightResultsReportCriteria criteria)
        {
            var setting = new ResultSetting { IsOutRight = true, LanguageCode = "en-US" };

            var resultsHtmlString = string.Empty;
            var cacheKey = GenerateOutRightCacheKey(criteria);
            var outRightData = (IList<OutRight>)HttpRuntime.Cache.Get(cacheKey) ?? _reportService.GetOutrightResults(criteria);

            if (outRightData.Count > 0)
            {
                var sports = outRightData.GroupBy(i => i.SportType)
                    .Select(j => new SelectListItem
                    {
                        Text = DataRefWrapper.GetSportName(j.First().SportType),
                        Value = j.First().SportType.ToString(),
                        Selected = j.First().SportType == criteria.SportType
                    }).OrderBy(k => k.Text).ToList();
                var sportType = criteria.SportType == 0 ? int.Parse(sports[0].Value) : criteria.SportType;
                var leagues = outRightData.Where(i => i.SportType == sportType)
                    .Select(j => new SelectListItem { Text = DataRefWrapper.GetLeagueName(j.LeagueId), Value = j.LeagueId.ToString() })
                    .OrderBy(k => k.Text).ToList();
                var data = outRightData.Where(i => i.SportType == sportType && (criteria.LeagueId == 0 || criteria.LeagueId == i.LeagueId)).ToList();
                if (data.Count > 0)
                {
                    resultsHtmlString = ResultEngine.HtmlParser(data, setting);
                }

                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    Sports = sports,
                    Leagues = leagues,
                    Results = resultsHtmlString,
                    TotalRows = data.Count
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Sports = new List<SelectListItem>(),
                Leagues = new List<SelectListItem>(),
                Results = ResultsOutrightNoData(),
                TotalRows = 0
            });
        }

        /// <summary>
        /// Get list of results.
        /// </summary>
        /// <param name="criteria">Results Report criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult GetMatchResults(MatchResultsReportCriteria criteria)
        {
            var leagueCriteria = new LeagueCriteria()
            {
                SportType = criteria.SportType,
                EventDate = criteria.EventDate
            };

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Results = GetMatchResultsData(leagueCriteria, criteria.LeagueId)
            });
        }

        /// <summary>
        /// Get list of leagues.
        /// </summary>
        /// <param name="criteria">League criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult GetLeagues(LeagueCriteria criteria)
        {
            var leagueItems = new List<SelectListItem>();
            var leagues = _reportService.GetLeagues(criteria);
            leagueItems = leagues.Select(i => new SelectListItem { Text = i.LeagueName, Value = i.LeagueID }).OrderBy(j => j.Text).ToList();

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Leagues = leagueItems,
                Results = GetMatchResultsData(criteria, 0)
            });
        }

        /// <summary>
        /// Get Balance up and move bet dates.
        /// </summary>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult GetBalanceUpMoveBet()
        {
            var balanceUp = _coreService.GetBalanceUpDay();
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                BalanceUp = balanceUp.BalanceUpDay.ToShortDateString(),
                MoveBet = balanceUp.MoveBetsDay.ToShortDateString()
            });
        }

        #endregion Results Report

        #region Agent Win Loss Report

        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <param name="agentId">Agent Id Info.</param>
        /// <param name="agentLevel">Agent Level.</param>
        /// <param name="breadCrumb">Bread Crumb.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult AgentWinLoss(int? agentId, int? agentLevel, string breadCrumb)
        {
            var paramAgentId = agentId ?? CurrentSession.AgentInfo.AgentId;
            var paramAgentLevel = agentLevel ?? CurrentSession.AgentInfo.AgentLevel;
            ViewBag.PageContext = new PageContext(paramAgentId, paramAgentLevel, CurrentSession.AgentInfo.NumberOfLevel)
            {
                IsDirectDownline = CurrentSession.IsDirectDownline(paramAgentLevel),
                IsAdmin = CurrentSession.IsAdmin
            };
            var model = new AgentWinLossReportViewModel
            {
                AgentId = paramAgentId,
                AgentLevel = paramAgentLevel,
                BreadCrumb = string.IsNullOrEmpty(breadCrumb) ? CurrentSession.AgentInfo.LicenseeName : breadCrumb
            };
            return View(model);
        }

        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <param name="model">Agent Win Loss Report View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult AgentWinLoss(AgentWinLossReportViewModel model)
        {
            var balanceUp = _coreService.GetBalanceUpDay();

            var criteria = new AgentWinLossReportCriteria
            {
                BalanceUpDay = balanceUp.BalanceUpDay,
                MoveBetDay = balanceUp.MoveBetsDay,
                DrawType = model.DrawFilter,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                AgentID = model.AgentId
            };

            var result = _reportService.AgentWinLossReport(criteria);

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = result
            });
        }

        #endregion Agent Win Loss Report

        #region Utilities

        /// <summary>
        /// Generate Outright Cache Key.
        /// </summary>
        /// <param name="criteria">Outright Results Report Criteria.</param>
        /// <returns>HTML string.</returns>
        private static string GenerateOutRightCacheKey(OutrightResultsReportCriteria criteria)
        {
            const string PatternKey = "{0}{1}Results.OutRight.Data_{2}";
            return string.Format(PatternKey, criteria.FromDate, criteria.ToDate, CurrentSession.OnUserId);
        }

        private static string ResultsMatchNoData()
        {
            var htmlNoDataStr = "<table class='fanex-result'><thead><tr class='row-header'><th class='column-kickofftime'>Kick Off. Time</th>";
            htmlNoDataStr += "<th class='column-match'>Match</th><th class='column-firsthaftscore'>First Half Score</th>";
            htmlNoDataStr += "<th class='column-finalscore'>Final Score</th><th class='column-status'>Status</th></tr></thead>";
            htmlNoDataStr += "<tbody><!--NoInfo--><tr class='row-noinfo'><td colspan='5'>There is no data</td></tr><!--/NoInfo--></tbody></table>";
            return htmlNoDataStr;
        }

        private static string ResultsOutrightNoData()
        {
            var htmlNoDataStr = "<table class='fanex-result'><thead><tr class='row-header'><th class='column-kickofftime'>Time</th>";
            htmlNoDataStr += "<th class='column-match'>Team</th><th class='column-status'>Resuls</th></tr></thead>";
            htmlNoDataStr += "<tbody><!--NoInfo--><tr class='row-noinfo'><td colspan='3'>There is no data</td></tr><!--/NoInfo--></tbody></table>";
            return htmlNoDataStr;
        }

        /// <summary>
        /// Get Match Results Data.
        /// </summary>
        /// <param name="criteria">Match Results Report Criteria.</param>
        /// <param name="leagueId">League ID input.</param>
        /// <returns>HTML string.</returns>
        private string GetMatchResultsData(LeagueCriteria criteria, int leagueId)
        {
            var matchResultsReportCriteria = new MatchResultsReportCriteria()
            {
                CustId = CurrentSession.OnUserId,
                SportType = criteria.SportType,
                LeagueId = leagueId,
                EventDate = criteria.EventDate
            };

            var setting = new ResultSetting
            {
                SportType = (SportType)criteria.SportType,
                LanguageCode = "en-US"
            };

            var data = _reportService.GetMatchResults(matchResultsReportCriteria);
            if (data != null)
            {
                return ResultEngine.HtmlParser(data, setting);
            }

            return string.Empty;
        }

        #endregion Utilities
    }
}