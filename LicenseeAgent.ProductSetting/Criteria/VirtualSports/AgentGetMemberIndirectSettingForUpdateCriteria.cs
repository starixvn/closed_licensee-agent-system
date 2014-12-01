namespace LicenseeAgent.ProductSetting.Criteria.VirtualSports
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentGetMemberIndirectSettingForUpdateCriteria class.
    /// </summary>
    public class AgentGetMemberIndirectSettingForUpdateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int AgentId { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetMemberIndirectForUpdateVirtualSports";
        }

        public override bool IsValid()
        {
            return CustId > 0 && AgentId > 0;
        }
    }
}