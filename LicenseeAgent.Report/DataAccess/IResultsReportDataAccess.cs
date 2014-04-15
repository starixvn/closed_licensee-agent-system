// ----------------------------------------------------------------------
// <copyright file="IResultsReportDataAccess.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Report.DataAccess
{
    using System.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Interface Account.
    /// </summary>
    public interface IResultsReportDataAccess
    {
        /// <summary>
        /// Get results.
        /// </summary>
        /// <param name="criteria">Results Criteria.</param>
        /// <returns>List of results.</returns>
        DataTable GetResults(ICriteria criteria);
    }
}