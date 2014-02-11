// ----------------------------------------------------------------------
// <copyright file="LogService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class LogService : QueryService, ILogService
    {
        /// <summary>
        /// Write log when call API.
        /// Wrap exception to avoid causing error on LAS site.
        /// </summary>
        /// <param name="criteria">Criteria store procedure.</param>
        public void WriteLog(ICriteria criteria)
        {
            try
            {
                Execute(criteria);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex);
            }
        }

        /// <summary>
        /// Get log informations.
        /// </summary>
        /// <param name="criteria">Criteria store procedure.</param>
        /// <returns>List log information.</returns>
        public IEnumerable<LogInformationEntity> GetLogInformations(ICriteria criteria)
        {
            return GetEntites<LogInformationEntity>(criteria);
        }
    }
}