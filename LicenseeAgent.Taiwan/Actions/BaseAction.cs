// ----------------------------------------------------------------------
// <copyright file="BaseAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Configurations;

    public abstract class BaseAction
    {
        protected BaseAction()
            : this(AppSettings.TaiwanAPI)
        {
        }

        protected BaseAction(string url)
        {
            Url = url;
        }

        [XmlIgnore]
        public string Url { get; private set; }

        public string SiteName { get; set; }

        public abstract string ActionName { get; }
    }
}