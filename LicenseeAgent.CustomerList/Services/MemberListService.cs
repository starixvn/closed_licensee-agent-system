namespace LicenseeAgent.CustomerList.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerList.Criteria.Member;
    using LicenseeAgent.CustomerList.Entities.Agent;
    using LicenseeAgent.CustomerList.Entities.Member;

    /// <summary>
    /// MemberService class.
    /// </summary>
    public class MemberListService : QueryService, IMemberListService
    {
        public IList<MemberEntity> GetMemberList(MemberGetByAgentCriteria criteria)
        {
            return GetEntites<MemberEntity>(criteria);
        }

        public IList<MemberEntity> SearchMember(SearchMemberCriteria criteria)
        {
            return GetEntites<MemberEntity>(criteria);
        }

        public void UpdateMemberAccStatus(UpdateMemberStatusCriteria criteria)
        {
            Execute(criteria);
        }

        public IList<MemberEntity> GetMemberAutoComplete(MemberAutoCompleteCriteria criteria)
        {
            return GetEntites<MemberEntity>(criteria);
        }

        public IList<AgentEntity> UplineAgentsByCustId(GetUplineByCustIdCriteria criteria)
        {
            return GetEntites<AgentEntity>(criteria);
        }

        public IList<MemberEntity> SearchMembersByIds(SearchMemberByIdsCriteria criteria)
        {
            return GetEntites<MemberEntity>(criteria);
        }

        public void CheckMemberUplineStatusForUpdate(CheckMemberUpLineStatusForUpdateCriteria criteria)
        {
            Execute(criteria);
        }
    }
}