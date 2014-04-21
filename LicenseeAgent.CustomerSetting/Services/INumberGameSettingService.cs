// ----------------------------------------------------------------------
// <copyright file="INumberGameSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Text;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.NumberGame;

    /// <summary>
    /// INumberGameSettingService interface.
    /// </summary>
    public interface INumberGameSettingService
    {
        /// <summary>
        /// Get Agent setting for insert new.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <returns>Number game customer setting.</returns>
        NumberGameCustomer GetNumberGameSettingForInsert(SettingContext settingContext);

        StringBuilder GetFormatJavascript(NumberGameCustomer customerSetting);
    }
}
