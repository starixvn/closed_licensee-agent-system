// ----------------------------------------------------------------------
// <copyright file="UpdateMemberStatusActionLog.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Entities
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [XmlRoot("Root")]
    public class UpdateMemberStatusActionLog
    {
        [XmlElement("Customer")]
        public List<UpdateMemberStatusLogEntity> CustomerLogs { get; set; }
    }
}