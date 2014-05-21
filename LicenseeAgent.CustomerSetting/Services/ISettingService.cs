// ----------------------------------------------------------------------
// <copyright file="ISettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// ISettingRepository interface.
    /// </summary>
    public interface ISettingService
    {
        Product ProductId { get; }

        string Name { get; }

        CustomerSetting2 GetSettingForInsert(SettingContext settingContext, out int errCode);

        CustomerSetting2 GetSettingForUpdate(SettingContext settingContext, out int errCode);
    }
}