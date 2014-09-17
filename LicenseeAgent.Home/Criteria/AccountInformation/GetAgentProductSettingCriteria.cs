namespace LicenseeAgent.Home.Criteria.AccountInformation
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetAgentProductSettingCriteria class.
    /// </summary>
    public class GetAgentProductSettingCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetProductSettingByAgentId";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && LicenseeId > 0 && CurrencyId > 0;
        }
    }
}