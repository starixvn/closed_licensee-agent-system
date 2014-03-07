// ----------------------------------------------------------------------
// <copyright file="MatchResultsReportCriteria.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Results Report Criteria.
    /// </summary>
    public class MatchResultsReportCriteria : ReportCriteriaBase
    {
        public int CustId { get; set; }

        public DateTime EventDate { get; set; }

        public int SportType { get; set; }

        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Result_MatchSelByLeague";
        }

        public override bool IsValid()
        {
            return EventDate != null;
        }
    }
}
