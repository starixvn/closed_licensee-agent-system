// --------------------------------------------------------------------------
// <copyright file="Formatter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>
    /// Formatter class.
    /// </summary>
    public static class Formatter
    {
        private static readonly CultureInfo EnglishCultureInfo = CultureInfo.GetCultureInfo("en-US");

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
        };

        public static string FormatInt(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:#,##0}", value);
        }

        public static string FormatDecimal(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:#,##0.00}", value);
        }

        public static string FormatDecimal4n(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:#,##0.####}", value);
        }

        public static string FormatPercent(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:#,##0.####%}", value);
        }

        public static string FormatDateTime(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:MM/dd/yyyy h:mm:ss tt}", value);
        }

        public static string FormatJson(string jsonName, object value)
        {
            return string.Format("var {0} = {1};\n", jsonName, JsonConvert.SerializeObject(value, JsonSerializerSettings));
        }

        public static string FormatJson(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}