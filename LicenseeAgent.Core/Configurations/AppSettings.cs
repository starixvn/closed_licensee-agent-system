namespace LicenseeAgent.Core.Configurations
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// Provide static accessing function for setting key from app setting.config.
    /// </summary>
    public static class AppSettings
    {
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        static AppSettings()
        {
            var downTimeValue = GetValue("DownTime");
            if (string.IsNullOrEmpty(downTimeValue))
            {
                IsDownTime = false;
            }
            else
            {
                IsDownTime = true;
                var downTimes = downTimeValue.Split('-');
                BeginTime = downTimes[0].Trim();
                EndTime = downTimes[1].Trim();
            }

            IsAdminSite = GetValue("IsAdminSite") == "1";
            var siteEnvironment = GetValue("SiteEnvironment").ToLower();
            switch (siteEnvironment)
            {
                case "prod":
                    SiteEnvironment = SiteEnvironment.Production;
                    break;

                case "uat":
                    SiteEnvironment = SiteEnvironment.Uat;
                    break;

                case "local":
                    SiteEnvironment = SiteEnvironment.Local;
                    break;

                default:
                    ThrowInvalidValueException("SiteEnvironment");
                    break;
            }

            UnderMaintenanceUrl = GetValue("UnderMaintenanceUrl");
            TaiwanAPIUrl = new Uri(GetValue("TaiwanAPIUrl"));
            TrustedAdmins = GetValue("TrustedAdmins");
            if (!RegLib.ValidateListOfString(TrustedAdmins))
            {
                TrustedAdmins = "*";
                Logger.Error("Key 'TrustedAdmins' has an invalid value");
            }

            OfferedProducts = GetValue("OfferedProducts");
            if (!RegLib.ValidateListOfInt(OfferedProducts))
            {
                OfferedProducts = "*";
                Logger.Error("Key 'OfferedProducts' has an invalid value");
            }
        }

        public static bool IsAdminSite { get; private set; }

        public static SiteEnvironment SiteEnvironment { get; private set; }

        public static string DomainEnvironment
        {
            get
            {
                switch (SiteEnvironment)
                {
                    case SiteEnvironment.Production:
                        return "PRO";

                    default:
                        return "UAT";
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Verified.")]
        public static string UnderMaintenanceUrl { get; private set; }

        public static bool IsDownTime { get; private set; }

        public static string BeginTime { get; private set; }

        public static string EndTime { get; private set; }

        public static bool IsLocalRequest
        {
            get
            {
                return SiteEnvironment == SiteEnvironment.Local && HttpContext.Current.Request.IsLocal;
            }
        }

        public static Uri TaiwanAPIUrl { get; private set; }

        public static string TrustedAdmins { get; private set; }

        public static string OfferedProducts { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key name.</param>
        /// <returns>The value of key name.</returns>
        private static string GetValue(string key)
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

        private static void ThrowInvalidValueException(string key)
        {
            var exception = new InvalidOperationException(string.Format("Invalid value of key {0}", key));
            Logger.HandleException(exception);
            throw exception;
        }
    }
}