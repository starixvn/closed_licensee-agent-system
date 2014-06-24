// ----------------------------------------------------------------------
// <copyright file="OutrightResultsReportCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Results Report Criteria.
    /// </summary>
    public class OutrightResultsReportCriteria : ReportCriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [SpParam(Ignored = true)]
        public int SportType { get; set; }

        [SpParam(Ignored = true)]
        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Result_Outright";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate;
        }
    }
}