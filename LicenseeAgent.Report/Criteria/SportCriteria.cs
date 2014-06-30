namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Sport Criteria.
    /// </summary>
    public class SportCriteria : CriteriaBase
    {
        public bool IsShowAll { get; set; }

        public DateTime EventDate { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Result_SportSelByDate";
        }

        public override bool IsValid()
        {
            return EventDate != null;
        }
    }
}