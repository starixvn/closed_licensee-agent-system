// ----------------------------------------------------------------------
// <copyright file="IMonitoringService.cs" company="Nexcel Solutions Vietnam">
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

    public interface IMonitoringService
    {
        IEnumerable<SyncMonitoring> GetFailedAPICalls(ICriteria criteria);

        void UpdateLogStatus(ICriteria criteria);

        void UpdateRunCount(ICriteria criteria);
    }
}