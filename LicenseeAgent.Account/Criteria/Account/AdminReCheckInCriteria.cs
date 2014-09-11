namespace LicenseeAgent.Account.Criteria.Account
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AdminReCheckInCriteria class.
    /// </summary>
    public class AdminReCheckInCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "On_AdminId")]
        public int OnAdminId { get; set; }

        [SpParam(SqlParamName = "On_LicenseeID")]
        public int? OnLicenseeId { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AdminReCheckIn";
        }

        public override bool IsValid()
        {
            return OnAdminId > 0;
        }
    }
}