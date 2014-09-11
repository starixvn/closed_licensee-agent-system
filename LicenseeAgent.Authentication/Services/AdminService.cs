namespace LicenseeAgent.Authentication.Services
{
    using LicenseeAgent.Authentication.Criteria;
    using LicenseeAgent.Authentication.Criteria.Admin;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// AdminService class.
    /// </summary>
    public class AdminService : QueryService, IAdminService
    {
        public string GetAdminSalt(AdminSaltSelectCriteria criteria)
        {
            Execute(criteria);
            return criteria.UserSalt;
        }

        /// <summary>
        /// Authenticates the admin.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return admin information.</returns>
        public AdminInfo AuthenticateAdmin(AdminLoginCriteria criteria)
        {
            Execute(criteria);
            if (criteria.ErrCode == Result.SuccessValue)
            {
                return new AdminInfo
                {
                    UserId = criteria.UserId,
                    UserName = criteria.UserName,
                    AccountType = AccountType.CSDIT
                };
            }

            return null;
        }

        public void AdminCheckIn(AdminCheckInCriteria criteria)
        {
            Execute(criteria);
        }
    }
}