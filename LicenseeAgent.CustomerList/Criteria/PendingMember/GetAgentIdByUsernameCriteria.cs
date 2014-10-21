namespace LicenseeAgent.CustomerList.Criteria.PendingMember
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetAgentIdByUsernameCriteria class.
    /// </summary>
    public class GetAgentIdByUsernameCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public int LicenseeId { get; set; }

        public int LicenseeCurrencyId { get; set; }

        public int AgentId { get; set; }
        
        public int UserId { get; set; }        

        public override string GetSettingKey()
        {
            return "Las_Cash_GetAgentIdByUsername";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName) && LicenseeId > 0 && LicenseeCurrencyId > 0;
        }
    }
}