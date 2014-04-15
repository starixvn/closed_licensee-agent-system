// --------------------------------------------------------------------------
// <copyright file="StatementAgentDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Statement Detail Of Agent Criteria.
    /// </summary>
    public class StatementAgentDetailCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string AgentUsername { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Statement_SelAgentStatement";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(AgentUsername);
        }
    }
}
