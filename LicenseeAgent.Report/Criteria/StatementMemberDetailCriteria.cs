// --------------------------------------------------------------------------
// <copyright file="StatementMemberDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.2.24">Created</change>
// </history>

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Statement Member Detail Criteria.
    /// </summary>
    public class StatementMemberDetailCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Statement_SelMemberStatement";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Username);
        }
    }
}
