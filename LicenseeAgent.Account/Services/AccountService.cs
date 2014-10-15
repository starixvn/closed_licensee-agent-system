namespace LicenseeAgent.Account.Services
{
    using LicenseeAgent.Account.Criteria.Account;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// Implements service functions related with account.
    /// </summary>
    public class AccountService : QueryService, IAccountService
    {
        public void AgentChangePassword(AgentChangePasswordCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentReCheckIn(AgentReCheckInCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentUserLogout(AgentUserLogoutCriteria criteria)
        {
            Execute(criteria);
        }

        public void AdminUserLogout(AdminUserLogoutCriteria criteria)
        {
            Execute(criteria);
        }

        public void AdminReCheckIn(AdminReCheckInCriteria criteria)
        {
            Execute(criteria);
        }
    }
}