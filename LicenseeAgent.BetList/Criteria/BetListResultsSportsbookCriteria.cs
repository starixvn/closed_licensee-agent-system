// ----------------------------------------------------------------------
// <copyright file="BetListResultsSportsbookCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/02/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class BetListResultsSportsbookCriteria : CriteriaBase
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
