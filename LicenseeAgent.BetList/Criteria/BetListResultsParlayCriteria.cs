// ----------------------------------------------------------------------
// <copyright file="BetListResultsParlayCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// BetListResultsParlayCriteria class.
    /// </summary>
    public class BetListResultsParlayCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "LAS_BetList_SelMatchResultParlay";
        }

        public override bool IsValid()
        {
            return RefNo > 0;
        }
    }
}