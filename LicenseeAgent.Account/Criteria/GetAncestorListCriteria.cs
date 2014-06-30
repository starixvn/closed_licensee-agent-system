namespace LicenseeAgent.Account.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetAncestorListCriteria class.
    /// </summary>
    public class GetAncestorListCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetAncestorList";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}