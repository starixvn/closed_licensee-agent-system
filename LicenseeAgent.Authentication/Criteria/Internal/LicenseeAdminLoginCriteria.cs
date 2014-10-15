namespace LicenseeAgent.Authentication.Criteria.Internal
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LicenseeAdminLoginCriteria class.
    /// </summary>
    public class LicenseeAdminLoginCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public string LoginIP { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public string UserName { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeAdminLogin";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0
                && UserId > 0
                && !string.IsNullOrEmpty(LoginIP);
        }
    }
}