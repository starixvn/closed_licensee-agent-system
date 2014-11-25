namespace LicenseeAgent.Report.Criteria.Results
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