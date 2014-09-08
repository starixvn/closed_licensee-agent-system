namespace LicenseeAgent.Account.Criteria.Account
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentChangePasswordCriteria class.
    /// </summary>
    public class AgentChangePasswordCriteria : CriteriaBase
    {
        public int Updater { get; set; }

        public int AgentId { get; set; }

        [SpParam(SqlParamName = "OldPwd")]
        public string OldPassword { get; set; }

        [SpParam(SqlParamName = "Pwd")]
        public string Password { get; set; }

        public string LoginIp { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentChangePwd";
        }

        public override bool IsValid()
        {
            return Updater > 0
                && AgentId > 0
                && !string.IsNullOrEmpty(OldPassword)
                && !string.IsNullOrEmpty(Password)
                && !string.IsNullOrEmpty(LoginIp);
        }
    }
}