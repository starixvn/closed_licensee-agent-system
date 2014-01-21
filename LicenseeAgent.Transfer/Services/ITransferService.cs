// --------------------------------------------------------------------------
// <copyright file="ITransferService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Transfer.Criteria;
    using LicenseeAgent.Transfer.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITransferService
    {
        IEnumerable<AgentBalanceInfo> GetAgentBalanceInfo(GetAgentBalanceInfoCriteria criteria);

        void TransferToAgent(TransferToAgentCriteria criteria);
    }
}