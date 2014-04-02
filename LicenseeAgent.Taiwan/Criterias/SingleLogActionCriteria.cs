// ----------------------------------------------------------------------
// <copyright file="SingleLogActionCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    using LicenseeAgent.Taiwan.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class SingleLogActionCriteria : LogActionCriteria
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// Status of API action.
        /// </value>
        public LogType Status { get; set; }

        public long LogId { get; set; }

        public long LogIdReturn { get; set; }
    }
}