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
    /// TransferService class.
    /// </summary>
    public class TransferService : QueryService, ITransferService
    {
        public IList<AgentBalanceInfo> GetAgentBalanceInfo(GetAgentBalanceInfoCriteria criteria)
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

        public MemberDetail GetMemberDetail(GetMemberDetailCriteria criteria)
        {
            return GetEntity<MemberDetail>(criteria);
        }

        public void TransferToMember(TransferToMemberCriteria criteria)
        {
            Execute(criteria);
        }

        public AgentTransferInfo GetTransferAgentDetail(GetTransferAgentDetailCriteria criteria)
        {
            return GetEntity<AgentTransferInfo>(criteria);
        }
    }
}