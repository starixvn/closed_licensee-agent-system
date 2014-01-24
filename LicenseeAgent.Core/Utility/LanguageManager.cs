// --------------------------------------------------------------------------
// <copyright file="LanguageManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System.Globalization;
    using System.Threading;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Manage supported languages.
    /// </summary>
    public static class LanguageManager
    {
        private static readonly CultureInfo USCulture = CultureInfo.CreateSpecificCulture("en-US");

        private static readonly System.Resources.ResourceManager _langResourceManager;

        static LanguageManager()
        {
            _langResourceManager = new System.Resources.ResourceManager(typeof(Las)) { IgnoreCase = true };
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
            if (!ReferenceEquals(_langResourceManager, null))
            {
                return _langResourceManager.GetString(key);
            }

            return string.Empty;
        }
    }
}