namespace LicenseeAgent.CustomerSetting.Criteria.NumberGame
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentGetMemberIndirectForUpdateCriteria class.
    /// </summary>
    public class AgentGetMemberSettingIndirectForUpdateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int AgentId { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetMemberNumberGameSettingIndirectForUpdate";
        }

        public override bool IsValid()
        {
            return CustId > 0 && AgentId > 0;
        }
    }
}