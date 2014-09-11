namespace LicenseeAgent.Authentication.Services
{
    using LicenseeAgent.Authentication.Criteria.Agent;
    using LicenseeAgent.Core.Entities;

    public interface IAgentService
    {
        AgentInfo AuthenticateAgent(AgentLoginCriteria criteria);

        string GetAgentSalt(AgentSaltSelectCriteria criteria);

        void AgentCheckIn(AgentCheckInCriteria criteria);
    }
}