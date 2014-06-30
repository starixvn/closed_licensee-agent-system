namespace LicenseeAgent.Transfer.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Transfer.Criteria;
    using LicenseeAgent.Transfer.Entities;

    public interface ITransferService
    {
        IList<AgentBalanceInfo> GetAgentBalanceInfo(GetAgentBalanceInfoCriteria criteria);

        void TransferToAgent(TransferToAgentCriteria criteria);

        IEnumerable<MemberBalanceInfo> GetMemberBalanceInfo(GetMemberBalanceInfoCriteria criteria);

        MemberDetail GetMemberDetail(GetMemberDetailCriteria criteria);

        void TransferToMember(TransferToMemberCriteria criteria);

        AgentTransferInfo GetTransferAgentDetail(GetTransferAgentDetailCriteria criteria);
    }
}