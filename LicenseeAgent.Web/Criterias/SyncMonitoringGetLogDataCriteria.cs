namespace LicenseeAgent.Web.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SyncMonitoringGetLogDataCriteria class.
    /// </summary>
    public class SyncMonitoringGetLogDataCriteria : CriteriaBase
    {
        public int KeyId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SyncMonitoring_GetLogData";
        }

        public override bool IsValid()
        {
            return KeyId > 0;
        }
    }
}