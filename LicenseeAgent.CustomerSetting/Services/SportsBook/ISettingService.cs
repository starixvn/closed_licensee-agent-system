// --------------------------------------------------------------------------
// <copyright file="ISettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services.SportsBook
{
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.CustomerSetting.Entities;

    public interface ISettingService
    {
        CustomerSetting2 GetSettingForInsert(SettingContext settingContext, out int errCode);

        CustomerSetting2 GetSettingForUpdate(SettingContext settingContext, out int errCode);
    }
}