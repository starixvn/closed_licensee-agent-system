namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICustomerSettingService
    {
        void CheckExistUserName(CheckExistUserNameCriteria criteria);

        InsertResult InsertCustomer(InsertCustomerViewModel model);

        IList<UpdateResult> UpdateCustomer(UpdateCustomerViewModel model);

        IList<UpdateResult> UpdateIndirectCustomer(UpdateCustomerViewModel model);

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