namespace LicenseeAgent.Report.Criteria.Results
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// League Criteria.
    /// </summary>
    public class GetLeagueCriteria : CriteriaBase
    {
        public bool IsShowAll { get; set; }

        public DateTime EventDate { get; set; }

        public int SportType { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Result_LeagueSelByDate";
        }

        public override bool IsValid()
        {
            return SportType > 0;
        }
    }
}