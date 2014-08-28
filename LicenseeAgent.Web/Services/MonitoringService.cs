namespace LicenseeAgent.Web.Services
{
    using System.Collections.Generic;
    using Entities;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Criteria;

    /// <summary>
    /// MonitoringService class.
    /// </summary>
    public class MonitoringService : QueryService, IMonitoringService
    {
        public IList<string> GetAPIUrls(ICriteria criteria)
        {
            return GetEntites<string>(criteria);
        }

        public IList<SyncMonitoring> GetFailedAPICalls(ICriteria criteria)
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

        public SyncMonitoring GetLogData(int keyId)
        {
            var criteria = new SyncMonitoringGetLogDataCriteria
            {
                KeyId = keyId
            };

            var logData = GetEntity<SyncMonitoring>(criteria);
            logData.KeyId = keyId;
            return logData;
        }

        /// <summary>
        /// Gets the drop down.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns list of drop down.</returns>
        public IList<SyncMonitoringAction> GetActions(ICriteria criteria)
        {
            return GetEntites<SyncMonitoringAction>(criteria);
        }
    }
}