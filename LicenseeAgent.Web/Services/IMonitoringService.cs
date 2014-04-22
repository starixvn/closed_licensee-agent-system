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
    using Criterias;
    using Entities;

    public interface IMonitoringService
    {
        IEnumerable<SyncMonitoring> GetFailedAPICalls(SyncMonitoringCriteria criteria);
    }
}