namespace LicenseeAgent.AccountInformation.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentSuggestSearchMemberCriteria class.
    /// </summary>
    public class AgentSuggestSearchMemberCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public int UserId { get; set; }

        [SpParam(SqlParamName = "StrSearch")]
        public string UserName { get; set; }

        public short LegendType { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SuggestSearchMember_Agent";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0
                && CurrencyId > 0
                && UserId > 0
                && !string.IsNullOrEmpty(UserName);
        }
    }
}