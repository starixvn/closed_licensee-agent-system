// ----------------------------------------------------------------------
// <copyright file="MonitoringService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/04/22">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Web.Services
{
    using System.Collections.Generic;
    using Entities;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// MonitoringService class.
    /// </summary>
    public class MonitoringService : QueryService, IMonitoringService
    {
        public IEnumerable<SyncMonitoring> GetFailedAPICalls(ICriteria criteria)
        {
            return GetEntites<SyncMonitoring>(criteria);
        }
        
        public void UpdateLogStatus(ICriteria criteria)
        {
            Execute(criteria);
        }

        public void UpdateRunCount(ICriteria criteria)
        {
            Execute(criteria);
        }
    }
}