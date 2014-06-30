namespace LicenseeAgent.Message.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Search Message Criteria.
    /// </summary>
    public class SearchMessageCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetAgentinfo_Detail";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate;
        }
    }
}