namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// MemberGetByAgentCriteria class.
    /// </summary>
    public class MemberGetByAgentCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MsgId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public override string GetSettingKey()
        {
            return "Las_MemberGetByAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}