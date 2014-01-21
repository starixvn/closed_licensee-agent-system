// --------------------------------------------------------------------------
// <copyright file="TransferToAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferToAgentCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int ToAgentId { get; set; }

        public decimal Amount { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Transfer_AgentToAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && ToAgentId > 0;
        }
    }
}