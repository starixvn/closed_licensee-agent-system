namespace LicenseeAgent.Web.Criteria
{
    using Fanex.Data.Repository;
    using LicenseeAgent.Taiwan.Enumerations;

    /// <summary>
    /// SyncMonitoringUpdateStatusCriteria class.
    /// </summary>
    public class SyncMonitoringUpdateStatusCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int KeyId { get; set; }

        public LogType NewStatusId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SyncMonitoring_UpdateLogStatus";
        }

        public override bool IsValid()
        {
            return KeyId > 0;
        }
    }
}