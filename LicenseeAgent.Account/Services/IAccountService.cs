namespace LicenseeAgent.Account.Services
{
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.Account.Criteria.Account;
    using LicenseeAgent.Core.Entities;

    public interface IAccountService
    {
        AgentInfo AuthenticateAgent(AgentLoginCriteria criteria);

        string GetAgentSalt(AgentSaltSelectCriteria criteria);

        void AgentCheckIn(AgentCheckInCriteria criteria);

        void AgentChangePassword(AgentChangePasswordCriteria criteria);

        void AgentReCheckIn(AgentReCheckInCriteria criteria);

        void AgentUserLogout(AgentUserLogoutCriteria criteria);
    }
}