// --------------------------------------------------------------------------
// <copyright file="CustomerSettingServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.29">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Services
{
    using LicenseeAgent.Core.Services;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerSettingServiceBase : QueryService
    {
        protected JsonSerializerSettings GetJsonSetting()
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            return settings;
        }
    }
}