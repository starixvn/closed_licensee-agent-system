namespace LicenseeAgent.CreditBalance.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentGetMemberRangeInfoCriteria class.
    /// </summary>
    public class AgentGetMemberRangeInfoCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal? MaxCreditOutput { get; set; }

        public decimal MinCreditOutput { get; set; }

        public decimal GivenCredit { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelMemberCredit";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}