namespace LicenseeAgent.CustomerSetting.Criteria.VirtualSports
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentGetMemberSettingForUpdateCriteria class.
    /// </summary>
    public class AgentGetMemberSettingForUpdateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetMemberVirtualSportsSettingForUpdate";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}