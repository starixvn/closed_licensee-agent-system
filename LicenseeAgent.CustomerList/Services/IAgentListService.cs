namespace LicenseeAgent.CustomerList.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerList.Criteria.Agent;
    using LicenseeAgent.CustomerList.Entities.Agent;

    /// <summary>
    /// AgentService class.
    /// </summary>
    public interface IAgentListService
    {
        IList<AgentEntity> GetAgentList(AgentGetByAgentCriteria criteria);

        IList<AgentEntity> SearchAgent(SearchAgentCriteria criteria);

        IList<AgentEntity> UpdateAgentAccStatus(UpdateAgentStatusCriteria criteria);

        IList<AgentEntity> GetAgentAutoComplete(AgentAutoCompleteCriteria criteria);

        AgentEntity GetAgentInfo(AgentInfoCriteria criteria);

        void ResetPassword(ResetPasswordAgentCriteria criteria);

        void EditInfo(EditinfoAgentCriteria criteria);
    }
}