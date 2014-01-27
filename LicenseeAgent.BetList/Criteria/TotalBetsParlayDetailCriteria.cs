// ----------------------------------------------------------------------
// <copyright file="TotalBetsParlayDetailCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014/1/17">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TotalBetsParlayDetailCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public long TransId { get; set; }

        [SpParam(Ignored = true)]
        public string Status { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Las_TotalBet_SelAgentPTComm";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && TransId > 0;
        }
    }
}
