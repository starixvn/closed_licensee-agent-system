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

    public interface ITransferService
    {
        IEnumerable<AgentBalanceInfo> GetAgentBalanceInfo(GetAgentBalanceInfoCriteria criteria);

        void TransferToAgent(TransferToAgentCriteria criteria);

        IEnumerable<MemberBalanceInfo> GetMemberBalanceInfo(GetMemberBalanceInfoCriteria criteria);

        MemberDetail GetMemberDetail(GetMemberDetailCriteria criteria);

        void TransferToMember(TransferToMemberCriteria criteria);
    }
}