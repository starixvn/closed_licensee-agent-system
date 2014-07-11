namespace LicenseeAgent.Web.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SyncMonitoringUpdateRunCountCriteria class.
    /// </summary>
    public class SyncMonitoringUpdateRunCountCriteria : CriteriaBase
    {
        public int KeyId { get; set; }

        public string Remark { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SyncMonitoring_UpdateRunCount";
        }

        public override bool IsValid()
        {
            return KeyId > 0;
        }
    }
}