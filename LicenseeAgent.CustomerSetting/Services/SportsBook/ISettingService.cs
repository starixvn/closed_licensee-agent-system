// --------------------------------------------------------------------------
// <copyright file="ISettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services.SportsBook
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.SportsBook;

    public interface ISettingService
    {
        CustomerSetting GetSettingForInsert(SettingContext settingContext, out int errCode);

        CustomerSetting GetSettingForUpdate(SettingContext settingContext, out int errCode);

        InsertResult InsertCustomer(ICriteria criteria);

        InsertResult UpdateCustomer(ICriteria criteria);

        InsertResult UpdateIndirectCustomer(ICriteria criteria);
    }
}