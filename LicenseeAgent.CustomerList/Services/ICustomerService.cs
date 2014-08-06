namespace LicenseeAgent.CustomerList.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerList.Criteria;
    using LicenseeAgent.CustomerList.Entities;

    public interface ICustomerService
    {
        /// <summary>
        /// Get agent list.
        /// </summary>
        /// <param name="criteria">Agent get by agent criteria.</param>
        /// <returns>List agent entity.</returns>
        IList<AgentEntity> GetAgentList(AgentGetByAgentCriteria criteria);

        /// <summary>
        /// Search agent.
        /// </summary>
        /// <param name="criteria">Search agent criteria.</param>
        /// <returns>List agent entity.</returns>
        IList<AgentEntity> SearchAgent(SearchAgentCriteria criteria);

        /// <summary>
        /// Update account status of agent.
        /// </summary>
        /// <param name="criteria">Update agent account status criteria.</param>
        /// <returns>List agent ID.</returns>
        IList<AgentEntity> UpdateAgentAccStatus(UpdateAgentStatusCriteria criteria);

        /// <summary>
        /// Get member list.
        /// </summary>
        /// <param name="criteria">Member get by agent criteria.</param>
        /// <returns>List member.</returns>
        IList<MemberEntity> GetMemberList(MemberGetByAgentCriteria criteria);

        /// <summary>
        /// Search member.
        /// </summary>
        /// <param name="criteria">Search member criteria.</param>
        /// <returns>List member.</returns>
        IList<MemberEntity> SearchMember(SearchMemberCriteria criteria);

        /// <summary>
        /// Update account status of member.
        /// </summary>
        /// <param name="criteria">Update member account status criteria.</param>
        void UpdateMemberAccStatus(UpdateMemberStatusCriteria criteria);

        /// <summary>
        /// Get agent auto complete.
        /// </summary>
        /// <param name="criteria">Agent get by agent criteria.</param>
        /// <returns>List Agent.</returns>
        IList<AgentEntity> GetAgentAutoComplete(AgentAutoCompleteCriteria criteria);

        /// <summary>
        /// Get member list.
        /// </summary>
        /// <param name="criteria">Member get by agent criteria.</param>
        /// <returns>List member.</returns>
        IList<MemberEntity> GetMemberAutoComplete(MemberAutoCompleteCriteria criteria);

        /// <summary>
        /// Reset Password.
        /// </summary>
        /// <param name="criteria">Reset Password Criteria.</param>
        void ResetPassword(ResetPasswordCriteria criteria);

        /// <summary>
        /// Edit Information Agent.
        /// </summary>
        /// <param name="criteria">Edit info Agent Criteria.</param>
        /// <returns>Agent Info.</returns>
        AgentEntity GetAgentInfo(AgentInfoCriteria criteria);

        /// <summary>
        /// Edit Information Agent.
        /// </summary>
        /// <param name="criteria">Edit info Agent Criteria.</param>
        void EditInfoAgent(EditinfoAgentCriteria criteria);

        void CheckMemberUplineStatusForUpdate(CheckMemberUpLineStatusForUpdateCriteria criteria);

        int GetAgentIdByUserName(GetAgentIdByUsernameCriteria criteria);

        IList<MemberEntity> SearchMembersByIds(SearchMemberByIdsCriteria criteria);
    }
}