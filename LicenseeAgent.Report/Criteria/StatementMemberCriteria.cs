// --------------------------------------------------------------------------
// <copyright file="StatementMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.2.21">Created</change>
// </history>

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Statement Member Criteria.
    /// </summary>
    public class StatementMemberCriteria : CriteriaBase
    {
        public int AgentID { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Statement_SelDownlineMembers";
        }

        public override bool IsValid()
        {
            return AgentID > 0;
        }
    }
}
