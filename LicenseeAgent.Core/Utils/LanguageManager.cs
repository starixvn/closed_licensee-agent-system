// --------------------------------------------------------------------------
// <copyright file="LanguageManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry" date="2013.6.3">Created</change>
//   <change who="Edward.Nguyen" date="2013.7.2">Change to static class</change>
// </history>

namespace LicenseeAgent.Core
{
    using System.Globalization;
    using System.Resources;
    using System.Threading;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Manage supported languages.
    /// </summary>
    public static class LanguageManager
    {
        private static readonly CultureInfo USCulture = CultureInfo.CreateSpecificCulture("en-US");
        private static readonly ResourceManager LangResourceManager;

        static LanguageManager()
        {
            LangResourceManager = new ResourceManager(typeof(Las)) { IgnoreCase = true };
        }

        public static string TwoLetterISOLanguageName
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            }
        }

        public static void SetCulture(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

                Thread.CurrentThread.CurrentCulture.DateTimeFormat = USCulture.DateTimeFormat;
                Thread.CurrentThread.CurrentCulture.NumberFormat = USCulture.NumberFormat;
            }
        }

        public static string GetResourceData(string key)
        {
            if (!ReferenceEquals(LangResourceManager, null))
            {
                return LangResourceManager.GetString(key);
            }

            return string.Empty;
        }
    }
}