// ----------------------------------------------------------------------
// <copyright file="IBetListDataAccess.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.DataAccess
{
    using Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.BetList.Criteria;

    /// <summary>
    /// Interface Account.
    /// </summary>
    public interface IBetListDataAccess
    {
        /// <summary>
        /// Get Bet List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        TicketResult GetBetList(ICriteria criteria);

        /// <summary>
        /// Get Bet List Parlay.
        /// </summary>
        /// <param name="criteria">Mix Parlay Details Criteria.</param>
        /// <returns>Ticket results.</returns>
        TicketResult GetMixParlayDetails(MixParlayDetailsCriteria criteria);

        /// <summary>
        /// Get system parlay result data.
        /// </summary>
        /// <param name="criteria">System parlay result criteria.</param>
        /// <returns>System Parlay Data.</returns>
        SystemParlayData GetSystemParlayResult(SystemParlayResultCriteria criteria);

        /// <summary>
        /// Get Parlay List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        TicketResult GetParlayList(ICriteria criteria);
    }
}