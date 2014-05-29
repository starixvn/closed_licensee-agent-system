namespace LicenseeAgent.Report.Criteria
{
    using System;

    /// <summary>
    /// Results Report Criteria.
    /// </summary>
    public class MatchResultsNumberGameCriteria : ReportCriteriaBase
    {
        public DateTime EventDate { get; set; }

        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Result_MatchSelNumberGameByLeague";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
