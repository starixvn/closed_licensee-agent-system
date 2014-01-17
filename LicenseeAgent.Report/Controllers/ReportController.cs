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
    using System.Web.Mvc;
    using Criteria;
    using Services;
    using Web.Controllers;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class ReportController : BaseController
    {
        #region Constant

        private bool IncludeTestData = false;

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
        /// Get list of sports.
        /// </summary>
        /// <param name="criteria">Sport criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpGet]
        public JsonResult GetSports(SportCriteria criteria)
        {
            criteria.IsShowAll = IncludeTestData;
            var sports = _resultsReportService.GetSports(criteria);

            return null;
        }

        /// <summary>
        /// Get list of leagues.
        /// </summary>
        /// <param name="criteria">League criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpGet]
        public JsonResult GetLeagues(LeagueCriteria criteria)
        {
            return null;
        }

        #endregion

        #region Utilities
        #endregion
    }
}
