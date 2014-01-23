// ----------------------------------------------------------------------
// <copyright file="LogService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class LogService : QueryService, ILogService
    {
        /// <summary>
        /// Write log when call API.
        /// </summary>
        /// <param name="criteria">Criteria store procedure.</param>
        public void WriteLog(ICriteria criteria)
        {
            Execute(criteria);
        }
    }
}
