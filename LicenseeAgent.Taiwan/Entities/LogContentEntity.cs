// ----------------------------------------------------------------------
// <copyright file="LogContentEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

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
    [XmlRootAttribute("Root", IsNullable = false)]
    public class LogContentEntity
    {
        [XmlElement(ElementName = "Url")]
        public string Link { get; set; }

        public string Action { get; set; }
        
        public LogAction Parameters { get; set; }
    }
}
