// --------------------------------------------------------------------------
// <copyright file="TransferService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Transfer.Criteria;
    using LicenseeAgent.Transfer.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferService : QueryService, ITransferService
    {
        public IEnumerable<AgentBalanceInfo> GetAgentBalanceInfo(GetAgentBalanceInfoCriteria criteria)
        {
            return GetEntites<AgentBalanceInfo>(criteria);
        }

        public void TransferToAgent(TransferToAgentCriteria criteria)
        {
            Execute(criteria);
        }

        public IEnumerable<MemberBalanceInfo> GetMemberBalanceInfo(GetMemberBalanceInfoCriteria criteria)
        {
            return GetEntites<MemberBalanceInfo>(criteria);
        }
    }
}