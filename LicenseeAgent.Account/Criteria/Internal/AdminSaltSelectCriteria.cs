namespace LicenseeAgent.Account.Criteria.Internal
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AdminSaltSelectCriteria class.
    /// </summary>
    public class AdminSaltSelectCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public string UserSalt { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AdminSaltSel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName);
        }
    }
}