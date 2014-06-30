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
            return true;
        }
    }
}