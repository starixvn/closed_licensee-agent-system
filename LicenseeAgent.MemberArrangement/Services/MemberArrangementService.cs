namespace LicenseeAgent.MemberArrangement.Services
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.MemberArrangement.Criteria;
    using LicenseeAgent.MemberArrangement.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberArrangementService : QueryService, IMemberArrangementService
    {
        public void MemberIsFirstDeposit(MemberIsFirstDepositCriteria criteria)
        {
            Execute(criteria);
        }

        public void MemberReArrangement(ReArrangementMemberCriteria criteria)
        {
            Execute(criteria);
        }

        public void SwitchUserFromNormalToLegend(SwitchUserFromNormalToLegendCriteria criteria)
        {
            Execute(criteria);
        }

        public void SwitchUserValidate(SwitchUserValidateCriteria criteria)
        {
            Execute(criteria);
        }

        public void GetLegendAgentId(GetLegendAgentIdCriteria criteria)
        {
            Execute(criteria);
        }

        public void GetLegendAgentIdHasDeposit(GetLegendAgentIdHasDepositCriteria criteria)
        {
            Execute(criteria);
        }

        public CustomerInfo GetMemberInfo(GetMemberInfoCriteria criteria)
        {
            return GetEntity<CustomerInfo>(criteria);
        }

        public AgentInfo GetAgentInfoByMember(GetAgentInfoByMemberCriteria criteria)
        {
            return GetEntity<AgentInfo>(criteria);
        }

        public AgentInformation GetAgentInfo(GetAgentInfoCriteria criteria)
        {
            return GetEntity<AgentInformation>(criteria);
        }
    }
}