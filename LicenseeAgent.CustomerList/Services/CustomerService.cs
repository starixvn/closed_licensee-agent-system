namespace LicenseeAgent.CustomerList.Services
{
    using System.Collections.Generic;
    using Fanex.Common;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerList.Criteria;
    using LicenseeAgent.CustomerList.Entities;

    /// <summary>
    /// CustomerService class.
    /// </summary>
    public class CustomerService : QueryService, ICustomerService
    {
        /// <summary>
        /// Get agent list.
        /// </summary>
        /// <param name="criteria">Agent get by agent criteria.</param>
        /// <returns>List agent entity.</returns>
        public IList<AgentEntity> GetAgentList(AgentGetByAgentCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        /// <summary>
        /// Search agent.
        /// </summary>
        /// <param name="criteria">Search agent criteria.</param>
        /// <returns>List agent entity.</returns>
        public IList<AgentEntity> SearchAgent(SearchAgentCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        /// <summary>
        /// Update account status of agent.
        /// </summary>
        /// <param name="criteria">Update agent account status criteria.</param>
        /// <returns>List agent ID.</returns>
        public IList<AgentEntity> UpdateAgentAccStatus(UpdateAgentStatusCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        /// <summary>
        /// Get member list.
        /// </summary>
        /// <param name="criteria">Member get by agent criteria.</param>
        /// <returns>List member.</returns>
        public IList<MemberEntity> GetMemberList(MemberGetByAgentCriteria criteria)
        {
            return GetEntites<MemberEntity>(criteria);
        }

        /// <summary>
        /// Search member.
        /// </summary>
        /// <param name="criteria">Search member criteria.</param>
        /// <returns>List member.</returns>
        public IList<MemberEntity> SearchMember(SearchMemberCriteria criteria)
        {
            return GetEntites<MemberEntity>(criteria);
        }

        /// <summary>
        /// Update account status of member.
        /// </summary>
        /// <param name="criteria">Update member account status criteria.</param>
        public void UpdateMemberAccStatus(UpdateMemberStatusCriteria criteria)
        {
            Execute(criteria);
        }

        /// <summary>
        /// Get agent auto complete.
        /// </summary>
        /// <param name="criteria">Agent get by agent criteria.</param>
        /// <returns>List Agent.</returns>
        public IList<AgentEntity> GetAgentAutoComplete(AgentAutoCompleteCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        /// <summary>
        /// Get member list.
        /// </summary>
        /// <param name="criteria">Member get by agent criteria.</param>
        /// <returns>List member.</returns>
        public IList<MemberEntity> GetMemberAutoComplete(MemberAutoCompleteCriteria criteria)
        {
            return GetEntites<MemberEntity>(criteria);
        }

        /// <summary>
        /// Reset Password.
        /// </summary>
        /// <param name="criteria">Reset Password Criteria.</param>
        public void ResetPassword(ResetPasswordCriteria criteria)
        {
            criteria.Password = Cryptography.CreateHash(criteria.Password, criteria.UserSalt);
            Execute(criteria);
        }

        public AgentEntity GetAgentInfo(AgentInfoCriteria criteria)
        {
            return GetEntity<AgentEntity>(criteria);
        }

        public void EditInfoAgent(EditinfoAgentCriteria criteria)
        {
            Execute(criteria);
        }

        public void CheckMemberUplineStatusForUpdate(CheckMemberUpLineStatusForUpdateCriteria criteria)
        {
            Execute(criteria);
        }

        public int GetAgentIdByUserName(GetAgentIdByUsernameCriteria criteria)
        {
            Execute(criteria);
            return criteria.AgentId;
        }
    }
}