namespace LicenseeAgent.Authentication.Criteria.Internal
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AdminCheckInLicenseeCriteria class.
    /// </summary>
    public class AdminCheckInLicenseeCriteria : CriteriaBase
    {
        public int OnAdminId { get; set; }

        public int LicenseeId { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AdminCheckInLicensee";
        }

        public override bool IsValid()
        {
            return OnAdminId > 0
                && LicenseeId > 0;
        }
    }
}