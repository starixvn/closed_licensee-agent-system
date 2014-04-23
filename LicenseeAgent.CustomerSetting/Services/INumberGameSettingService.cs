// ----------------------------------------------------------------------
// <copyright file="INumberGameSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// INumberGameSettingService interface.
    /// </summary>
    public interface INumberGameSettingService
    {
        /// <summary>
        /// Get Agent setting for insert new.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>
        /// Number game customer setting.
        /// </returns>
        Entities.NumberGame.CustomerSetting GetNumberGameSettingForInsert(SettingContext settingContext, out int errCode);
    }
}