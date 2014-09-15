namespace LicenseeAgent.ProductSetting.Criteria
{
    /// <summary>
    /// Criteria information.
    /// </summary>
    public class AgentUpdateAgentCriteria : UpdateCustomerCriteria
    {
        public AgentUpdateAgentCriteria(UpdateCustomerCriteria criteria)
            : base(criteria)
        {
        }

        public int AgentId { get; set; }

        /// <summary>
        /// Get Setting key.
        /// </summary>
        /// <returns>Current setting key.</returns>
        public override string GetSettingKey()
        {
            return "Las_AgentEditAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}