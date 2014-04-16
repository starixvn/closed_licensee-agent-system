// ----------------------------------------------------------------------
// <copyright file="BetListResultCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Criteria
{
    /// <summary>
    /// BetListResultCriteria class.
    /// </summary>
    public class BetListResultCriteria
    {
        public bool IsSportsBook { get; set; }

        public int MatchId { get; set; }

        public long RefNo { get; set; }

        public int SportType { get; set; }
    }
}
