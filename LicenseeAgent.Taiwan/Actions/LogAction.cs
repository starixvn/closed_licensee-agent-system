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
        [Service(IsParamAction = true)]
        [XmlIgnore]
        public int LicenseeId { get; set; }

        [Service(IsParamAction = true)]
        [XmlIgnore]
        public int UserId { get; set; }
    }
}
