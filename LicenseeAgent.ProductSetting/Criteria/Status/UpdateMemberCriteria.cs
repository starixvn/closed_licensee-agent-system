namespace LicenseeAgent.ProductSetting.Criteria.Status
{
    using Fanex.Data.Repository;

    /// <summary>
    /// UpdateMemberProductSettingCriteria class.
    /// </summary>
    public class UpdateMemberCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public int ProductId { get; set; }

        public bool Disable { get; set; }

        public int UpdatedBy { get; set; }

        public string Ip { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_UpdateProductSettingByCustId";
        }

        public override bool IsValid()
        {
            return CustId > 0
                && LicenseeId > 0
                && CurrencyId > 0
                && ProductId > 0
                && UpdatedBy > 0
                && !string.IsNullOrEmpty(Ip);
        }
    }
}