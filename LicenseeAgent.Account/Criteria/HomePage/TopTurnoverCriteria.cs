namespace LicenseeAgent.Account.Criteria.HomePage
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TopTurnoverCriteria class.
    /// </summary>
    public class TopTurnoverCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelTopTurnoverMembers";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}