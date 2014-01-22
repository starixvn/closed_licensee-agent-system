// ----------------------------------------------------------------------
// <copyright file="LogAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class LogAction
    {
        [Service(IsWriteLog = true)]
        public int LicenseeId { get; set; }

        [Service(IsWriteLog = true)]
        public int UserId { get; set; }
    }
}
