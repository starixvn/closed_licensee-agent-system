// ----------------------------------------------------------------------
// <copyright file="IResultsReportService.cs" company="Nexcel Solutions Vietnam">
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
    using Criteria;
    using Entities;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IResultsReportService
    {
        /// <summary>
        /// Get Sports.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List of sports.</returns>
        IEnumerable<Sport> GetSports(SportCriteria criteria);

       /// <summary>
        /// Get Leagues.
        /// </summary>
        /// <param name="criteria">Input Criteria.</param>
        /// <returns>List of leagues.</returns>
        IEnumerable<League> GetLeagues(LeagueCriteria criteria);

        /// <summary>
        /// Get match results.
        /// </summary>
        /// <param name="criteria">System parlay result criteria.</param>
        void GetMatchResults(ICriteria criteria);

        /// <summary>
        /// Get outright results.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        void GetOutrightResults(ICriteria criteria);
    }
}
