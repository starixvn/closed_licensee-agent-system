namespace LicenseeAgent.Home.Criteria.AccountInformation
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetDownLineListCriteria class.
    /// </summary>
    public class GetDownLineListCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetDownlineList";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}