// ----------------------------------------------------------------------
// <copyright file="AgentBalanceCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class AgentBalanceCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public decimal TotalCash { get; set; }

        public decimal YesterdayCash { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal TotalOutstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal UsedCredit { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Balance_SelAgentBalance";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
