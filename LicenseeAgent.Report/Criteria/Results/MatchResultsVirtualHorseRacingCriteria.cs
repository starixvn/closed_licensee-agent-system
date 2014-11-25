namespace LicenseeAgent.Report.Criteria.Results
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchResultsVirtualHorseRacingCriteria : ReportCriteriaBase
    {
        public DateTime EventDate { get; set; }

        public int SportType { get; set; }

        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Result_VirtualHRSelByMeeting";
        }

        public override bool IsValid()
        {
            return SportType > 0;
        }
    }
}