// ----------------------------------------------------------------------
// <copyright file="IReportService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using Criteria;
    using Entities;
    using Fanex.NAP.Framework.Data;
    using Fanex.Result.Core.DTO;

    public interface IReportService
    {
        /// <summary>
        /// Get Sports.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List of sports.</returns>
        IList<Sport> GetSports(SportCriteria criteria);

       /// <summary>
        /// Get Leagues.
        /// </summary>
        /// <param name="criteria">Input Criteria.</param>
        /// <returns>List of leagues.</returns>
        IList<League> GetLeagues(LeagueCriteria criteria);

        /// <summary>
        /// Get match results.
        /// </summary>
        /// <param name="criteria">System parlay result criteria.</param>
        /// <returns>List of match results.</returns>
        IList<Sportbook> GetMatchResults(ICriteria criteria);

        /// <summary>
        /// Get outright results.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>List of outright results.</returns>
        IList<OutRight> GetOutrightResults(ICriteria criteria);

        /// <summary>
        /// Get outstanding results.
        /// </summary>
        /// <param name="criteria">Outstanding Criteria.</param>
        /// <returns>List of agent outstanding results.</returns>
        IList<AgentOutstanding> GetOutstandings(ICriteria criteria);

        IList<AgentWinLoss> AgentWinLossReport(ICriteria criteria);

        IList<Statement> StatementReport(ICriteria criteria);

        IList<Transfer> TransferDetail(ICriteria criteria);
    }
}
