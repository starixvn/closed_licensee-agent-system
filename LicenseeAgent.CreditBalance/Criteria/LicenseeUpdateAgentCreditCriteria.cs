// --------------------------------------------------------------------------
// <copyright file="LicenseeUpdateAgentCreditCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseeUpdateAgentCreditCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public decimal Credit { get; set; }

        public decimal? MaxCreditOutput { get; set; }

        public decimal MinCreditOutput { get; set; }

        public int MsgId { get; set; }

        public int Updater { get; set; }

        public string Ip { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeUpdAgentCredit";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}