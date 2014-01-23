// -----------------------------------------------------------------------
// <copyright file="LogContentEntity.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [XmlRootAttribute("root", IsNullable = false)]
    public class LogContentEntity
    {
        public string Url { get; set; }

        public string Action { get; set; }
        
        public LogAction Parameters { get; set; }
    }
}
