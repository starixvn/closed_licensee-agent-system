// --------------------------------------------------------------------------
// <copyright file="AppURLs.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.5.13">Created</change>
// </history>

namespace LicenseeAgent.Web
{
    using System.Configuration;

    //// http://haacked.com/archive/2007/03/11/custom-configuration-sections-in-3-easy-steps.aspx
    
    /// <summary>
    /// Custom URLs for application.
    /// </summary>
    public class AppURLs : ConfigurationSection
    {
        private static AppURLs settings = ConfigurationManager.GetSection("LicenseeAgent.AppURLs") as AppURLs;

        public static AppURLs URLs
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("ChangePassword", IsRequired = true)]
        public string ChangePassword
        {
            get { return (string)this["ChangePassword"]; }
            set { this["ChangePassword"] = value; }
        }

        [ConfigurationProperty("AccessDenied", IsRequired = true)]
        public string AccessDenied
        {
            get { return (string)this["AccessDenied"]; }
            set { this["AccessDenied"] = value; }
        }

        [ConfigurationProperty("AccountStopped", IsRequired = true)]
        public string AccountStopped
        {
            get { return (string)this["AccountStopped"]; }
            set { this["AccountStopped"] = value; }
        }

        [ConfigurationProperty("Error", IsRequired = true)]
        public string Error
        {
            get { return (string)this["Error"]; }
            set { this["Error"] = value; }
        }

        [ConfigurationProperty("Unauthorized", IsRequired = true)]
        public string Unauthorized
        {
            get { return (string)this["Unauthorized"]; }
            set { this["Unauthorized"] = value; }
        }

        [ConfigurationProperty("UnderMaintenance", IsRequired = true)]
        public string UnderMaintenance
        {
            get { return (string)this["UnderMaintenance"]; }
            set { this["UnderMaintenance"] = value; }
        }
    }
}