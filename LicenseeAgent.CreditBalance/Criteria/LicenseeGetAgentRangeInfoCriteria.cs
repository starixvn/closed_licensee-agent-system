namespace LicenseeAgent.CreditBalance.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LicenseeGetAgentRangeInfoCriteria class.
    /// </summary>
    public class LicenseeGetAgentRangeInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public decimal? MaxCreditOutput { get; set; }

        public decimal MinCreditOutput { get; set; }

        public decimal GivenCredit { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeSelAgentCredit";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}