// ----------------------------------------------------------------------
// <copyright file="LogType.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum LogType : short
    {
        /// <summary>
        /// Before call API.
        /// </summary>
        BeforeCall = 1,

        /// <summary>
        /// After call API, response result is successful.
        /// </summary>
        Success = 2,

        /// <summary>
        /// After call API, response result is failure.
        /// </summary>
        Fail = 3
    }
}
