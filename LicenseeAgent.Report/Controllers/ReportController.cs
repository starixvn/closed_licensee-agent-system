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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Criteria;
    using Fanex.Result.Core;
    using Fanex.Result.Core.DTO;
    using LicenseeAgent.Core.Entities;
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

        #endregion

        #region Services

        private IResultsReportService _resultsReportService;

        #endregion

        #region Constructor

        public ReportController(IResultsReportService resultsReportService)
        {
            _resultsReportService = resultsReportService;
        }

        #endregion

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
            var sports = _resultsReportService.GetSports(criteria);
            var model = new ResultsReportViewModel();
            foreach (var item in sports)
            {
                model.Sports.Add(new SelectListItem { Text = item.SportName, Value = item.SportType });
            }

            if (model.Sports.Count > 0)
            {
                model.Sports[0].Selected = true;
                var leagueCriteria = new LeagueCriteria()
                {
                    IsShowAll = criteria.IsShowAll,
                    EventDate = criteria.EventDate,
                    SportType = int.Parse(model.Sports[0].Value)
                };

                var leagues = _resultsReportService.GetLeagues(leagueCriteria);
                foreach (var leagueItem in leagues)
                {
                    model.Leagues.Add(new SelectListItem { Text = leagueItem.LeagueName, Value = leagueItem.LeagueID });
                }

                var resultsReportCriteria = new MatchResultsReportCriteria()
                {
                    CustId = CurrentSession.OnUserId,
                    EventDate = criteria.EventDate,
                    SportType = int.Parse(model.Sports[0].Value),
                    LeagueId = int.Parse(model.Leagues[0].Value)
                };

                var resultsData = GetMatchResultsData(resultsReportCriteria);
                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    Sports = model.Sports,
                    Leagues = model.Leagues
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Sports = new List<SelectListItem>(),
                Leagues = new List<SelectListItem>(),
                Results = "No data."
            });
        }

        /// <summary>
        /// Get list of results.
        /// </summary>
        /// <param name="criteria">Results Report criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public ActionResult GetMatchResults(MatchResultsReportCriteria criteria)
        {
            ViewBag.ResultsData = GetMatchResultsData(criteria);
            return View("Report/Results");
        }

        /// <summary>
        /// Get list of results.
        /// </summary>
        /// <param name="criteria">Results Report criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public ActionResult GetOutrightResults(OutrightResultsReportCriteria criteria)
        {
            ViewBag.ResultsData = GetOutrightResultsData(criteria);
            return View("Report/Results");
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
            var leagues = _resultsReportService.GetLeagues(criteria);
            foreach (var leagueItem in leagues)
            {
                leagueItems.Add(new SelectListItem { Text = leagueItem.LeagueName, Value = leagueItem.LeagueID });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Leagues = leagueItems
            });
        }

        #endregion

        #region Utilities
        
        /// <summary>
        /// Get Match Results Data.
        /// </summary>
        /// <param name="criteria">Match Results Report Criteria.</param>
        /// <returns>HTML string.</returns>
        private string GetMatchResultsData(MatchResultsReportCriteria criteria)
        {
            var setting = new ResultSetting
            {
                SportType = (SportType)criteria.SportType,
                LanguageCode = "en-US"
            };

            var data = _resultsReportService.GetMatchResults(criteria);
            if (data != null)
            {
                return ResultEngine.HtmlParser(data, setting);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Outright Results Data.
        /// </summary>
        /// <param name="criteria">Outright Results Report Criteria.</param>
        /// <returns>HTML string.</returns>
        private string GetOutrightResultsData(OutrightResultsReportCriteria criteria)
        {
            var setting = new ResultSetting
            {
                IsOutRight = true,
                LanguageCode = "en-US"
            };
            var cacheKey = GenerateOutRightCacheKey(criteria);
            var outRightData = (IList<OutRight>)HttpRuntime.Cache.Get(cacheKey) ?? _resultsReportService.GetOutrightResults(criteria);
            if (outRightData != null)
            {
                var data =
                    outRightData.Where(i => i.SportType == criteria.SportType && (criteria.LeagueId == 0 || criteria.LeagueId == i.LeagueId)).ToList();
                if (data.Count > 0)
                {
                    return ResultEngine.HtmlParser(data, setting);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Generate Outright Cache Key.
        /// </summary>
        /// <param name="criteria">Outright Results Report Criteria.</param>
        /// <returns>HTML string.</returns>
        private string GenerateOutRightCacheKey(OutrightResultsReportCriteria criteria)
        {
            const string PatternKey = "{0}{1}Result.OutRight.Data_{2}";
            return string.Format(PatternKey, criteria.FromDate, criteria.ToDate, CurrentSession.OnUserId);
        }

        #endregion
    }
}
