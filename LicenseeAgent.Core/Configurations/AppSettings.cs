// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.12">Created</change>
//   <change who="Anders.Le" date="2013.6.12">Change IsMultipleSignOn to IsAllowMultipleSignIn</change>
// </history>

namespace LicenseeAgent.Core.Configurations
{
    using System;
    using System.Configuration;

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

            IsInternal = GetValue("IsInternal") == "1";
            UnderMaintenanceUrl = GetValue("UnderMaintenaceUrl");
            DisabledUnderMaintenanceAPI = GetValue("DisabledUnderMaintenanceAPI") == "1";
            LocalTest = GetValue("LocalTest") == "1";
            IsDownTime = GetValue("IsDownTime") == "1";
        }

        public static bool IsInternal { get; private set; }

        public static string UnderMaintenanceUrl { get; private set; }

        public static bool DisabledUnderMaintenanceAPI { get; private set; }

        public static bool IsDownTime { get; private set; }

        public static string BeginTime { get; private set; }

        public static string EndTime { get; private set; }

        public static bool LocalTest { get; private set; }

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
            catch (Exception ex)
            {
                Fanex.Logging.LogService.Error(ex.Message, ex);
                return string.Empty;
            }
        }
    }
}