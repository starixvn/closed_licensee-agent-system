namespace LicenseeAgent.Account.Criteria.HomePage
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TopWinningCriteria class.
    /// </summary>
    public class TopWinningCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelTopWinningMembers";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}