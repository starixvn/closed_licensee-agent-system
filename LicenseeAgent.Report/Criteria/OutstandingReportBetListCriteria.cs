// ----------------------------------------------------------------------
// <copyright file="OutstandingReportBetListCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/02/13">Created</change>
//   <change who="" date=""></change>
// </history>

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
