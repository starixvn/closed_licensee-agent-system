// -----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// ENUM Extensions class.
    /// </summary>
    public static class EnumExtensions
    {
        public static string EnumToJson(this Type type)
        {
            var results = Enum.GetValues(type).Cast<object>()
                                                .ToDictionary(enumValue => enumValue.ToString(), enumValue => (int)enumValue);

            return JsonConvert.SerializeObject(results);
        }
    }
}
