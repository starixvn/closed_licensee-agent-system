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
    using Fanex.Data.Repository;

    public interface IMonitoringService
    {
        IList<SyncMonitoringAPIUrl> GetAPIUrls(ICriteria criteria);

        IList<SyncMonitoring> GetFailedAPICalls(ICriteria criteria);

        SyncMonitoring GetLogData(int keyId);

        void UpdateLogStatus(ICriteria criteria);

        void UpdateRunCount(ICriteria criteria);
    }
}