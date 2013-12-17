// -----------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerService : QueryService, ICustomerService
    {
        /// <summary>
        /// Get agent list.
        /// </summary>
        /// <param name="criteria">Agent get by agent criteria.</param>
        /// <returns>List agent entity.</returns>
        public IEnumerable<AgentEntity> GetAgentList(AgentGetByAgentCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        /// <summary>
        /// Search agent.
        /// </summary>
        /// <param name="criteria">Search agent criteria.</param>
        /// <returns>List agent entity.</returns>
        public IEnumerable<AgentEntity> SearchAgent(SearchAgentCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        /// <summary>
        /// Update account status of agent.
        /// </summary>
        /// <param name="criteria">Update agent account status criteria.</param>
        public void UpdateAgentAccStatus(UpdateAgentAccStatusCriteria criteria)
        {
            Execute(criteria);
        }

        /// <summary>
        /// Get member list.
        /// </summary>
        /// <param name="criteria">Member get by agent criteria.</param>
        /// <returns>List member.</returns>
        public IEnumerable<MemberEntity> GetMemberList(MemberGetByAgentCriteria criteria)
        {
            return GetEntites<MemberEntity>(criteria);
        }

        /// <summary>
        /// Search member.
        /// </summary>
        /// <param name="criteria">Search member criteria.</param>
        /// <returns>List member.</returns>
        public IEnumerable<MemberEntity> SearchMember(SearchMemberCriteria criteria)
        {
            return GetEntites<MemberEntity>(criteria);
        }

        /// <summary>
        /// Update account status of member.
        /// </summary>
        /// <param name="criteria">Update member account status criteria.</param>
        public void UpdateMemberAccStatus(UpdateMemberAccStatusCriteria criteria)
        {
            Execute(criteria);
        }
        
        /// <summary>
        /// Get agent auto complete.
        /// </summary>
        /// <param name="criteria">Agent get by agent criteria.</param>
        /// <returns>List Agent.</returns>
        public IEnumerable<AgentEntity> GetAgentAutoComplete(AgentAutoCompleteCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        /// <summary>
        /// Get member list.
        /// </summary>
        /// <param name="criteria">Member get by agent criteria.</param>
        /// <returns>List member.</returns>
        public IEnumerable<MemberEntity> GetMemberAutoComplete(MemberAutoCompleteCriteria criteria)
        {
            return GetEntites<MemberEntity>(criteria);
        }
    }
}
