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
    using Criterias;
    using Entities;

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
        TicketResult GetBetList(BetListCriteria criteria);

        /// <summary>
        /// Get Bet List Parlay.
        /// </summary>
        /// <param name="transId">Input transaction ID.</param>
        /// <returns>Ticket results.</returns>
        TicketResult GetBetListParlay(long transId);

        #endregion
    }
}
