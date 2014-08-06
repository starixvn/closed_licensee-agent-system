namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SearchMemberByIdsCriteria class.
    /// </summary>
    public class SearchMemberByIdsCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string ListCustId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SearchMemberByListId";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && !string.IsNullOrEmpty(ListCustId);
        }
    }
}
