namespace LicenseeAgent.CreditBalance.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentUpdateMemberCreditCriteria class.
    /// </summary>
    public class AgentUpdateMemberCreditCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal Credit { get; set; }

        public int MsgId { get; set; }

        public int Updater { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentUpdMemberCredit";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}