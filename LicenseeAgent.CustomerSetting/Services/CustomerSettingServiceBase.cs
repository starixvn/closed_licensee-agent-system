// --------------------------------------------------------------------------
// <copyright file="CustomerSettingServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using LicenseeAgent.Core.Services;
    using Newtonsoft.Json;

    /// <summary>
    /// CustomerSettingServiceBase class.
    /// </summary>
    public class CustomerSettingServiceBase : QueryService
    {
        protected static JsonSerializerSettings GetJsonSetting
        {
            get
            {
                return new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented
                };
            }
        }
    }
}