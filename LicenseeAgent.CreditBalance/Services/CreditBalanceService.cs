// --------------------------------------------------------------------------
// <copyright file="CreditBalanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CreditBalance.Criteria;
    using LicenseeAgent.CreditBalance.Entities;

    public class CreditBalanceService : QueryService, ICreditBalanceService
    {
        public IEnumerable<AgentBalanceInfo> GetAgentBalanceInfo(GetAgentBalanceInfoCriteria criteria)
        {
            return GetEntites<AgentBalanceInfo>(criteria);
        }

        public IEnumerable<MemberBalanceInfo> GetMemberBalanceInfo(GetMemberBalanceInfoCriteria criteria)
        {
            return GetEntites<MemberBalanceInfo>(criteria);
        }

        public void LicenseeGetAgentRangeInfo(LicenseeGetAgentRangeInfoCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentGetMemberRangeInfo(AgentGetMemberRangeInfoCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentUpdateMemberCredit(AgentUpdateMemberCreditCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentGetAgentRangeInfo(AgentGetAgentRangeInfoCriteria criteria)
        {
            Execute(criteria);
        }

        public void UpdateAgentCredit(UpdateAgentCreditCriteria criteria)
        {
            Execute(criteria);
        }
    }
}