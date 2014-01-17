// ----------------------------------------------------------------------
// <copyright file="ResultsReportService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/16">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using Core.Services;
    using Criteria;
    using Entities;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResultsReportService : QueryService, IResultsReportService
    {
        #region Fields

        #endregion

        #region Constructor
        
        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Sports.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List of sports.</returns>
        public IEnumerable<Sport> GetSports(SportCriteria criteria)
        {
            return GetEntites<Sport>(criteria);
        }

        /// <summary>
        /// Get Leagues.
        /// </summary>
        /// <param name="criteria">Input Criteria.</param>
        /// <returns>List of leagues.</returns>
        public IEnumerable<League> GetLeagues(LeagueCriteria criteria)
        {
            return GetEntites<League>(criteria);
        }

        /// <summary>
        /// Get match results.
        /// </summary>
        /// <param name="criteria">System parlay result criteria.</param>
        public void GetMatchResults(ICriteria criteria)
        {

        }

        /// <summary>
        /// Get outright results.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        public void GetOutrightResults(ICriteria criteria)
        {

        }
        
        #endregion
    }
}
