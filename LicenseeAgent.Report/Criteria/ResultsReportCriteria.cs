// ----------------------------------------------------------------------
// <copyright file="ResultsReportCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/16">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Results Report Criteria.
    /// </summary>
    public class ResultsReportCriteria : ReportCriteriaBase
    {
        public int CustId { get; set; }

        [SpParam(Ignored = true)]
        public bool ShowMatchResults { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime EventDate { get; set; }

        public int SportType { get; set; }

        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return ShowMatchResults
                ? "LAS_Result_MatchSelByLeague"
                : "LAS_Result_Outright";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && EventDate != null;
        }
    }
}
