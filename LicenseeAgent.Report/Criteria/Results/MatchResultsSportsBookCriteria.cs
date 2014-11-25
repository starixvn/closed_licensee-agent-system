namespace LicenseeAgent.Report.Criteria.Results
{
    using System;

    /// <summary>
    /// Results Report Criteria.
    /// </summary>
    public class MatchResultsSportsBookCriteria : ReportCriteriaBase
    {
        public DateTime EventDate { get; set; }

        public int SportType { get; set; }

        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Result_MatchSelByLeague";
        }

        public override bool IsValid()
        {
            return SportType > 0;
        }
    }
}