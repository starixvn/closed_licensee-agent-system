// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Configurations
{
    using System;
    using System.Configuration;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// Provide static accessing function for setting key from app setting.config.
    /// </summary>
    public static class AppSettings
    {
        static AppSettings()
        {
            var downTimes = GetValue("DownTime").Split('-');
            BeginTime = downTimes[0].Trim();
            EndTime = downTimes[1].Trim();

            IsAdminSite = GetValue("IsAdminSite") == "1";
            Environment = GetValue("Environment");
            UnderMaintenanceUrl = GetValue("UnderMaintenanceUrl");
            LocalTest = GetValue("LocalTest") == "1";
            LocalLicenseeId = Convert.ToInt32(GetValue("LocalLicenseeId"));
            IsDownTime = GetValue("IsDownTime") == "1";
            TaiwanAPI = GetValue("TaiwanAPI");
            TrustedAdmins = GetValue("TrustedAdmins");
            if (!RegLib.ValidateListOfString(TrustedAdmins))
            {
                throw new InvalidOperationException("Key 'TrustedAdmins' has an invalid value");
            }

            OfferedProducts = GetValue("OfferedProducts");
            if (!RegLib.ValidateListOfInt(OfferedProducts))
            {
                throw new InvalidOperationException("Key 'OfferedProducts' has an invalid value");
            }
        }

        public static bool IsAdminSite { get; private set; }

        public static string Environment { get; private set; }

        public static string UnderMaintenanceUrl { get; private set; }

        public static bool IsDownTime { get; private set; }

        public static string BeginTime { get; private set; }

        public static string EndTime { get; private set; }

        public static bool LocalTest { get; private set; }

        public static int LocalLicenseeId { get; private set; }

        public static string TaiwanAPI { get; private set; }

        public static string TrustedAdmins { get; private set; }

        public static string OfferedProducts { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key name.</param>
        /// <returns>The value of key name.</returns>
        public static string GetValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToLower();
            }
            catch
            {
                var exception = new InvalidOperationException(string.Format("Key '{0}' not found in appSettings.", key));
                Logger.HandleException(exception);
                throw exception;
            }
        }
    }
}