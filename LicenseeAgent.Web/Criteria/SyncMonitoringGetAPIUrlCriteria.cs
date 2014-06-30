namespace LicenseeAgent.Web.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SyncMonitoringGetAPIUrlCriteria class.
    /// </summary>
    public class SyncMonitoringGetAPIUrlCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Las_SyncMonitoring_GetAPIUrls";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}