// --------------------------------------------------------------------------
// <copyright file="ISportsbookSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.8">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Services
{
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.Sportsbook;
    using LicenseeAgent.CustomerSetting.ViewModels;

    public interface ISportsbookSettingService
    {
        void CheckExistUserName(CheckExistUserNameCriteria criteria);

        SportsbookCustomer GetSportsbookSettingForInsert(SettingContext settingContext);

        SportsbookCustomer GetSportsbookSettingForUpdate(SettingContext settingContext);

        InsertResult InsertCustomer(InsertCustomerViewModel model);

        InsertResult UpdateCustomer(UpdateCustomerViewModel model);

        InsertResult UpdateIndirectCustomer(UpdateCustomerViewModel model);

        string FormatJavascriptReturn(
            SettingContext settingContext,
            SportsbookCustomer sportsBookCustomer);
    }
}