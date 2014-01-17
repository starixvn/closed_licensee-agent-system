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
    using System.Web.Mvc;
    using Criteria;
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
            var model = new ResultsReportViewModel();
            return View(model);
        }

        /// <summary>
        /// Get list of sports.
        /// </summary>
        /// <param name="criteria">Sport criteria.</param>
        /// <returns>JSON object.</returns>
        public ActionResult GetSports(SportCriteria criteria)
        {
            criteria.IsShowAll = _includeTestData;
            var sports = _resultsReportService.GetSports(criteria);

            var model = new ResultsReportViewModel();
            foreach (var item in sports)
            {
                model.Sports.Add(new SelectListItem { Text = item.SportName, Value = item.SportType });
            }

            return View("Results", model);
        }

        /// <summary>
        /// Get list of leagues.
        /// </summary>
        /// <param name="criteria">League criteria.</param>
        /// <returns>JSON object.</returns>
        public JsonResult GetLeagues(LeagueCriteria criteria)
        {
            return null;
        }

        #endregion

        #region Utilities
        #endregion
    }
}
