namespace LicenseeAgent.CustomerList.Criteria.Agent
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SearchAgentCriteria class.
    /// </summary>
    public class SearchAgentCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string UserName { get; set; }

        public int? AccStatus { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SearchAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}