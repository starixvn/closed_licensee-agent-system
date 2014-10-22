namespace LicenseeAgent.CustomerList.Criteria.Member
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SearchMemberCriteria class.
    /// </summary>
    public class SearchMemberCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string UserName { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SearchMember";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}