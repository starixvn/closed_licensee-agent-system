namespace LicenseeAgent.Authentication.Criteria.Admin
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AdminLoginCriteria class.
    /// </summary>
    public class AdminLoginCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        [SpParam(Ignored = true)]
        public string UserSalt { get; set; }

        public string UserPwd { get; set; }

        public string LoginIp { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public int ErrCode { get; set; }

        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AdminLogin";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName)
                && !string.IsNullOrEmpty(UserPwd)
                && !string.IsNullOrEmpty(UserSalt)
                && !string.IsNullOrEmpty(LoginIp);
        }
    }
}