// --------------------------------------------------------------------------
// <copyright file="LanguageManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Manage supported languages.
    /// </summary>
    public static class LanguageManager
    {
        private static readonly CultureInfo USCulture = CultureInfo.CreateSpecificCulture("en-US");

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