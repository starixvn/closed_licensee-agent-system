// ----------------------------------------------------------------------
// <copyright file="LogAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    using System;
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Criterias;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public abstract class LogAction : BaseAction
    {
        public LogAction()
        {
            Url = AppSettings.TaiwanAPI;
        }

        public string Url { get; set; }

        [Service]
        [XmlIgnore]
        public int LicenseeId { get; set; }

        [Service]
        [XmlIgnore]
        public int UserId { get; set; }

        public abstract LogActionCriteria CreateCriteria();

        protected string GetDoingLogXml()
        {
            return ObjectSerializer<LogAction>.Serialize(this);
        }
    }
}