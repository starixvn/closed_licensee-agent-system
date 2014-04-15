// --------------------------------------------------------------------------
// <copyright file="TransferDetailAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Transfer Detail Agent Criteria.
    /// </summary>
    public class TransferDetailAgentCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Statement_AgentTransferDetail";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
