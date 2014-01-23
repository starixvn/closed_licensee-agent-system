// ----------------------------------------------------------------------
// <copyright file="ILogService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Services
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal interface ILogService
    {
        /// <summary>
        /// Write log when call API.
        /// </summary>
        /// <param name="criteria">Criteria store procedure.</param>
        void WriteLog(ICriteria criteria);
    }
}
