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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICreditBalanceService
    {
        IEnumerable<AgentBalanceInfo> GetAgentBalanceInfo(GetAgentBalanceInfoCriteria criteria);

        IEnumerable<MemberBalanceInfo> GetMemberBalanceInfo(GetMemberBalanceInfoCriteria criteria);
    }
}