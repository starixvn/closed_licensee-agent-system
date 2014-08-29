namespace LicenseeAgent.AccountInformation.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AdminGetMemberListCriteria class.
    /// </summary>
    public class AdminGetMemberListCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int AgentId { get; set; }

        public int UserId { get; set; }

        public int CurrencyId { get; set; }

        [SpParam(SqlParamName = "StrSearch")]
        public string UserName { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }

        [SpParam(SqlParamName = "returnCurrency")]
        public int AgentCurrency { get; set; }

        [SpParam(SqlParamName = "returnCurrencyName")]
        public string AgentCurrencyName { get; set; }

        public short LegendType { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetMemberList_Admin";
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