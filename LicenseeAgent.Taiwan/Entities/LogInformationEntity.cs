// ----------------------------------------------------------------------
// <copyright file="LogInformationEntity.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LogInformationEntity
    {
        [XmlAttribute(AttributeName = "MemberID")]
        [Service(Name = "IBCCustID")]
        public int MemberId { get; set; }

        [XmlAttribute(AttributeName = "AgentID")]
        public int AgentId { get; set; }

        [XmlAttribute(AttributeName = "LogID")]
        public int LogId { get; set; }

        [XmlAttribute(AttributeName = "StatusID")]
        public short Status { get; set; }
    }
}
