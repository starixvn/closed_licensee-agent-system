// ----------------------------------------------------------------------
// <copyright file="LogAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public abstract class LogAction
    {
        [Service(IsParamAction = false)]
        [XmlIgnore]
        public int LicenseeId { get; set; }

        [Service(IsParamAction = false)]
        [XmlIgnore]
        public int UserId { get; set; }
    }
}
