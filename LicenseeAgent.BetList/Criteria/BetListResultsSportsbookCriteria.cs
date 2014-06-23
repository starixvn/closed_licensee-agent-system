// ----------------------------------------------------------------------
// <copyright file="BetListResultsSportsBookCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// BetListResultsSportsBookCriteria class.
    /// </summary>
    public class BetListResultsSportsBookCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "LAS_BetList_SelMatchResult";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}