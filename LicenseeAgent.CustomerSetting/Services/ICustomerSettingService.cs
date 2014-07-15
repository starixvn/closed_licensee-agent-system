namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
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

        bool MemberIsFirstDeposit(MemberIsFirstDepositCriteria criteria);

        void MemberReArrangement(ReArrangementMemberCriteria criteria);

        void SwitchUserFromNormalToLegend(SwitchUserFromNormalToLegendCriteria criteria);
    }
}