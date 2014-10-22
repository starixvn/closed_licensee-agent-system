namespace LicenseeAgent.ProductSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// CheckExistUserNameCriteria class.
    /// </summary>
    public class CheckExistUserNameCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public string UserName { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_CheckExistUserName";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0 &&
                CurrencyId > 0 &&
                !string.IsNullOrEmpty(UserName);
        }
    }
}