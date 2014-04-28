// --------------------------------------------------------------------------
// <copyright file="ICreditBalanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CreditBalance.Criteria;
    using LicenseeAgent.CreditBalance.Entities;

    public interface ICreditBalanceService
    {
        IList<AgentBalanceInfo> GetAgentBalanceInfo(GetAgentBalanceInfoCriteria criteria);

        IList<MemberBalanceInfo> GetMemberBalanceInfo(GetMemberBalanceInfoCriteria criteria);

        void LicenseeGetAgentRangeInfo(LicenseeGetAgentRangeInfoCriteria criteria);

        void AgentGetMemberRangeInfo(AgentGetMemberRangeInfoCriteria criteria);

        void AgentUpdateMemberCredit(AgentUpdateMemberCreditCriteria criteria);

        void AgentGetAgentRangeInfo(AgentGetAgentRangeInfoCriteria criteria);

        void UpdateAgentCredit(UpdateAgentCreditCriteria criteria);
    }
}