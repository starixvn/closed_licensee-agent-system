namespace LicenseeAgent.Web.Services
{
    using System.Collections.Generic;
    using Entities;
    using Fanex.Data.Repository;

    public interface IMonitoringService
    {
        IList<string> GetAPIUrls(ICriteria criteria);

        IList<SyncMonitoring> GetFailedAPICalls(ICriteria criteria);

        SyncMonitoring GetLogData(int keyId);

        void UpdateLogStatus(ICriteria criteria);

        void UpdateRunCount(ICriteria criteria);
    }
}