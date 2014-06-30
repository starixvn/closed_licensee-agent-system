namespace LicenseeAgent.Transfer.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMemberBalanceInfoCriteria class.
    /// </summary>
    public class GetMemberBalanceInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Transfer_MemberBalanceInfo";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}