// --------------------------------------------------------------------------
// <copyright file="Formatter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Utils
{
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Formatter
    {
        private static CultureInfo englishCultureInfo = CultureInfo.GetCultureInfo("en-US");

        private static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
        };

        public static string FormatInt(object value)
        {
            return string.Format(englishCultureInfo.NumberFormat, "{0:#,##0}", value);
        }

        public static string FormatPercent(object value)
        {
            return string.Format(englishCultureInfo.NumberFormat, "{0:P}", value);
        }

        public static string FormatJson(string jsonName, object value)
        {
            return string.Format("var {0} = {1};\n", jsonName, JsonConvert.SerializeObject(value, jsonSerializerSettings));
        }

        public static string FormatJson(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}