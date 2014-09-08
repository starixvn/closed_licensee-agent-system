namespace LicenseeAgent.Account.Criteria.Internal
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AdminUserLogoutCriteria class.
    /// </summary>
    public class AdminUserLogoutCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "On_AdminId")]
        public int OnAdminId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AdminUserLogout";
        }

        public override bool IsValid()
        {
            return OnAdminId > 0;
        }
    }
}