namespace LicenseeAgent.Core.Utility
{
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Manage supported languages.
    /// </summary>
    public static class LanguageManager
    {
        public const string UsLanguageCode = "en-US";
        private static readonly CultureInfo USCulture = CultureInfo.CreateSpecificCulture(UsLanguageCode);

        public static string TwoLetterISOLanguageName
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            }
        }

        public static string Language
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.Name;
            }

            set
            {
                var language = value;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

                Thread.CurrentThread.CurrentCulture.DateTimeFormat = USCulture.DateTimeFormat;
                Thread.CurrentThread.CurrentCulture.NumberFormat = USCulture.NumberFormat;
            }
        }
    }
}