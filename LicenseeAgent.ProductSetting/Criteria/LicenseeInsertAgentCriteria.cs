namespace LicenseeAgent.ProductSetting.Criteria
{
    /// <summary>
    /// LicenseeInsertAgentCriteria class.
    /// </summary>
    public class LicenseeInsertAgentCriteria : InsertCustomerCriteria
    {
        public LicenseeInsertAgentCriteria(InsertCustomerCriteria criteria)
            : base(criteria)
        {
        }

        public string UserSalt { get; set; }

        public int AgentL0Id { get; set; }

        public int AgentId { get; set; }

        public string UserPwd { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public int Status { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeInsertAgent";
        }

        public override bool IsValid()
        {
            return base.IsValid()
                && AgentL0Id > 0
                && !string.IsNullOrEmpty(UserSalt)
                && !string.IsNullOrEmpty(UserPwd);
        }
    }
}