// ----------------------------------------------------------------------
// <copyright file="LogType.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Enumerations
{
    using System.Xml.Serialization;

    public enum LogType
    {
        /// <summary>
        /// None status.
        /// </summary>
        [XmlEnum("0")]
        None = 0,

        /// <summary>
        /// Before call API.
        /// </summary>
        [XmlEnum("1")]
        BeforeCall = 1,

        /// <summary>
        /// After call API, response result is successful.
        /// </summary>
        [XmlEnum("2")]
        Success = 2,

        /// <summary>
        /// After call API, response result is failure.
        /// </summary>
        [XmlEnum("3")]
        Fail = 3
    }
}