namespace LicenseeAgent.Home.Criteria.AccountInformation
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetDirectDownLineCriteria class.
    /// </summary>
    public class GetDirectDownLineCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetDirectDownline";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}