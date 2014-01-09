// ----------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Services
{
    using System.Collections.Generic;
    using Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.BetList.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBetListService
    {
        #region BetList

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
        /// Get Bet List Details.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>A list of bet list details.</returns>
        IEnumerable<RawBetListDetails> GetBetListDetails(BetListDetailsCriteria criteria);
     
        #endregion
    }
}
