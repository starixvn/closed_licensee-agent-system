namespace LicenseeAgent.CustomerList.Services
{
    using System.Collections.Generic;
    using Fanex.Security;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerList.Criteria.Agent;
    using LicenseeAgent.CustomerList.Entities.Agent;

    /// <summary>
    /// AgentService class.
    /// </summary>
    public class AgentListService : QueryService, IAgentListService
    {
        public IList<AgentEntity> GetAgentList(AgentGetByAgentCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        public IList<AgentEntity> SearchAgent(SearchAgentCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        /// <summary>
        /// Updates the agent account status and get list of agents who has direct members.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns list of agents.</returns>
        public IList<AgentEntity> UpdateAgentAccStatus(UpdateAgentStatusCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        public IList<AgentEntity> GetAgentAutoComplete(AgentAutoCompleteCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        public AgentEntity GetAgentInfo(AgentInfoCriteria criteria)
        {
            return GetEntity<AgentEntity>(criteria);
        }

        public void ResetPassword(ResetPasswordAgentCriteria criteria)
        {
            criteria.Password = Cryptography.CreateHash(criteria.Password, criteria.UserSalt);
            Execute(criteria);
        }

        public void EditInfo(EditinfoAgentCriteria criteria)
        {
            Execute(criteria);
        }
    }
}