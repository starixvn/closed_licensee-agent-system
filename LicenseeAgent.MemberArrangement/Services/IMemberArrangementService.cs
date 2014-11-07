namespace LicenseeAgent.MemberArrangement.Services
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.MemberArrangement.Criteria;
    using LicenseeAgent.MemberArrangement.Entities;

    /// <summary>
    /// ICustomerSettingService interface.
    /// </summary>
    public interface IMemberArrangementService
    {
        void MemberIsFirstDeposit(MemberIsFirstDepositCriteria criteria);

        void MemberReArrangement(ReArrangementMemberCriteria criteria);

        void SwitchUserFromNormalToLegend(SwitchUserFromNormalToLegendCriteria criteria);

        void SwitchUserValidate(SwitchUserValidateCriteria criteria);

        void GetLegendAgentId(GetLegendAgentIdCriteria criteria);

        void GetLegendAgentIdHasDeposit(GetLegendAgentIdHasDepositCriteria criteria);

        CustomerInfo GetMemberInfo(GetMemberInfoCriteria criteria);

        AgentInfo GetAgentInfoByMember(GetAgentInfoByMemberCriteria criteria);

        AgentInformation GetAgentInfo(GetAgentInfoCriteria criteria);
    }
}