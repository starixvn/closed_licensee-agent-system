// ----------------------------------------------------------------------
// <copyright file="BetListDetailsCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/25">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListDetailsCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public long TransId { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "LAS_Betlist_SelAgentPTComm";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && TransId > 0;
        }
    }
}
