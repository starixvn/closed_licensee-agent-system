// ----------------------------------------------------------------------
// <copyright file="IResultsReportDataAccess.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/21">Created</change>
//   <change who="" date=""></change>
// </history>

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