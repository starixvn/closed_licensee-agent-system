namespace LicenseeAgent.Account.Services
{
    using LicenseeAgent.Account.Criteria.Account;

    public interface IAccountService
    {
        void AgentChangePassword(AgentChangePasswordCriteria criteria);

        void AgentReCheckIn(AgentReCheckInCriteria criteria);

        void AgentUserLogout(AgentUserLogoutCriteria criteria);

        void AdminUserLogout(AdminUserLogoutCriteria criteria);

        void AdminReCheckIn(AdminReCheckInCriteria criteria);
    }
}