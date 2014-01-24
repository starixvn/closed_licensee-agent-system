// -----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
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
