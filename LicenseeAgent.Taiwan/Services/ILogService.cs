// ----------------------------------------------------------------------
// <copyright file="ILogService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Services
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Taiwan.Entities;

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

        /// <summary>
        /// Get list log information.
        /// </summary>
        /// <param name="criteria">Criteria store procedure.</param>
        /// <returns>List log information.</returns>
        IEnumerable<UpdateMemberStatusLogEntity> GetUpdateMemberStatusLogs(ICriteria criteria);
    }
}