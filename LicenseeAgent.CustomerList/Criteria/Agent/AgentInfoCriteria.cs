namespace LicenseeAgent.CustomerList.Criteria.Agent
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentInfoCriteria class.
    /// </summary>
    public class AgentInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public int CurrencyId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetAgentinfo_Detail";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}