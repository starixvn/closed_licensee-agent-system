// --------------------------------------------------------------------------
// <copyright file="TransferToMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferToMemberCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int ToCustId { get; set; }

        public string Ip { get; set; }

        public decimal Amount { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Transfer_AgentToMember";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && ToCustId > 0;
        }
    }
}