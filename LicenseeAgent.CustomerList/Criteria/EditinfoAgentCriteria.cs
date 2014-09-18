namespace LicenseeAgent.Customer.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Edit info Agent Criteria.
    /// </summary>
    public class EditinfoAgentCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public int AgentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Editinfo_Agent";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}