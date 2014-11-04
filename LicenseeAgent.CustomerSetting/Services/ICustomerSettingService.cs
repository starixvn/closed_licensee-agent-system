namespace LicenseeAgent.MemberReArrangement.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.MemberReArrangement.Criteria;
    using LicenseeAgent.MemberReArrangement.Entities;
    using LicenseeAgent.MemberReArrangement.ViewModels;

    /// <summary>
    /// ICustomerSettingService interface.
    /// </summary>
    public interface ICustomerSettingService
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