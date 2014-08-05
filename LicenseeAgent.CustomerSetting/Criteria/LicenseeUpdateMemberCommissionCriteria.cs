namespace LicenseeAgent.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LicenseeUpdateMemberCommissionCriteria class.
    /// </summary>
    public class LicenseeUpdateMemberCommissionCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        [SpParam(SqlParamName = "strCustIDOriginal")]
        public string OrginalCustomer { get; set; }

        public int AdminId { get; set; }

        [SpParam(SqlParamName = "SBComm")]
        public decimal SBCommission { get; set; }

        [SpParam(SqlParamName = "NGComm")]
        public decimal NGCommission { get; set; }

        [SpParam(SqlParamName = "RacingComm")]
        public decimal RacingCommission { get; set; }

        [SpParam(SqlParamName = "LCComm")]
        public decimal LCCommission { get; set; }

        [SpParam(SqlParamName = "RNGComm")]
        public decimal RNGCommission { get; set; }

        public bool IsUpdateNG { get; set; }

        public bool IsUpdateRacing { get; set; }

        public bool IsUpdateLC { get; set; }

        public bool IsUpdateRNG { get; set; }

        public string Ip { get; set; }

        [SpParam(SqlParamName = "LangKey")]
        public string LanguageKey { get; set; }

        [SpParam(SqlParamName = "strCustID")]
        public string EffectedCustomers { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeUpdMemberComm";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0 && AdminId > 0 && !string.IsNullOrEmpty(OrginalCustomer);
        } 
    }
}
