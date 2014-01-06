// ----------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Services
{
    using Core.Entities;
    using Core.Services;
    using Criterias;
    using DataAccess;
    using Entities;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListService : QueryService, IBetListService
    {
        #region Fields

        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly IBetListDataAccess _betlistDataAccess;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        public BetListService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        /// <param name="betlistDataAccess">Account data access.</param>
        private BetListService(IBetListDataAccess betlistDataAccess)
        {
            _betlistDataAccess = betlistDataAccess ?? new BetListDataAccess();
        }

        #endregion Constructor

        #region BetList

        /// <summary>
        /// Get Bet List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        public TicketResult GetBetList(ICriteria criteria)
        {
            return _betlistDataAccess.GetBetList(criteria);
        }

        /// <summary>
        /// Get Bet List Parlay.
        /// </summary>
        /// <param name="agentId">Input agent ID.</param>
        /// <param name="transId">Input transaction ID.</param>
        /// <returns>Ticket results.</returns>
        public TicketResult GetMixParlayDetails(int agentId, long transId)
        {
            return _betlistDataAccess.GetMixParlayDetails(agentId, transId);
        }

        #endregion
    }
}
