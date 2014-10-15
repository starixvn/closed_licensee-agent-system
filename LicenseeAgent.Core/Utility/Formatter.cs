namespace LicenseeAgent.Core.Utility
{
    using System.Globalization;
    using LicenseeAgent.Core.Configurations;
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

        public static string FormatDecimal4(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:#,##0.####}", value);
        }

        public static string FormatPercent(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:#,##0.####%}", value);
        }

        public static string FormatDate(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:MM/dd/yyyy}", value);
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

        public static string FormatMediumUserName(string userName)
        {
            if (userName.Length > Constants.MediumStringLength)
            {
                return string.Format("{0}...", userName.Substring(0, Constants.MediumStringLength));
            }

            return userName;
        }
    }
}