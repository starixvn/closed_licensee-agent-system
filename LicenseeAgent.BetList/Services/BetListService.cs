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
    using System.Collections.Generic;
    using Core.Services;
    using DataAccess;
    using Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.BetList.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListService : QueryService, IBetListService
    {
        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly IBetListDataAccess _betlistDataAccess;

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
        /// <param name="criteria">Mix Parlay Details Criteria.</param>
        /// <returns>Ticket results.</returns>
        public TicketResult GetMixParlayDetails(MixParlayDetailsCriteria criteria)
        {
            return _betlistDataAccess.GetMixParlayDetails(criteria);
        }

        /// <summary>
        /// Get system parlay result data.
        /// </summary>
        /// <param name="criteria">System parlay result criteria.</param>
        /// <returns>System Parlay Data.</returns>
        public SystemParlayData GetSystemParlayResult(SystemParlayResultCriteria criteria)
        {
            return _betlistDataAccess.GetSystemParlayResult(criteria);
        }
        
        /// <summary>
        /// Get Bet List Details.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>A list of bet list details.</returns>
        public IEnumerable<RawBetListDetails> GetBetListDetails(BetListDetailsCriteria criteria)
        {
            return GetEntites<RawBetListDetails>(criteria);
        }
        
        /// <summary>
        /// Get Parlay List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        public TicketResult GetParlayList(ICriteria criteria)
        {
            return _betlistDataAccess.GetParlayList(criteria);
        }
    }
}
