// ----------------------------------------------------------------------
// <copyright file="StatusEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "Root")]
    public class StatusEntity
    {
        [XmlElement(ElementName = "Customer")]
        public Collection<LogInformationEntity> Customers { get; set; }
    }
}
