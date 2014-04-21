// --------------------------------------------------------------------------
// <copyright file="ISportsBookSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Text;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.SportsBook;
    using LicenseeAgent.CustomerSetting.ViewModels;

    public interface ISportsBookSettingService
    {
        void CheckExistUserName(CheckExistUserNameCriteria criteria);

        SportsBookCustomer GetSportsBookSettingForInsert(SettingContext settingContext);

        SportsBookCustomer GetSportsBookSettingForUpdate(SettingContext settingContext);

        InsertResult InsertCustomer(InsertCustomerViewModel model);

        InsertResult UpdateCustomer(UpdateCustomerViewModel model);

        InsertResult UpdateIndirectCustomer(UpdateCustomerViewModel model);

        StringBuilder GetFormatJavascript(
            SportsBookCustomer customerSetting,
            SettingContext settingContext);
    }
}