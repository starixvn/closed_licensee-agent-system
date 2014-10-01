namespace LicenseeAgent.CustomerList.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerList.Criteria.Member;
    using LicenseeAgent.CustomerList.Entities.Agent;
    using LicenseeAgent.CustomerList.Entities.Member;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMemberListService
    {
        IList<MemberEntity> GetMemberList(MemberGetByAgentCriteria criteria);

        IList<MemberEntity> SearchMember(SearchMemberCriteria criteria);

        void UpdateMemberAccStatus(UpdateMemberStatusCriteria criteria);

        IList<MemberEntity> GetMemberAutoComplete(MemberAutoCompleteCriteria criteria);

        IList<AgentEntity> UplineAgentsByCustId(GetUplineByCustIdCriteria criteria);

        IList<MemberEntity> SearchMembersByIds(SearchMemberByIdsCriteria criteria);

        void CheckMemberUplineStatusForUpdate(CheckMemberUpLineStatusForUpdateCriteria criteria);
    }
}