// ----------------------------------------------------------------------
// <copyright file="OutstandingReportBetListCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Outstanding Report Bet List Criteria.
    /// </summary>
    public class OutstandingReportBetListCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int CustId { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "Las_BetlistOutStanding_SelByMember";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && CustId > 0;
        }
    }
}