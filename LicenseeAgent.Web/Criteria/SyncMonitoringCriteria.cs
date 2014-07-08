namespace LicenseeAgent.Web.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SyncMonitoringCriteria class.
    /// </summary>
    public class SyncMonitoringCriteria : CriteriaBase
    {
        public int ActionId { get; set; }

        public string TaiwanAPIUrl { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SyncMonitoring";
        }

        public override bool IsValid()
        {
            return ActionId >= 0 && !string.IsNullOrEmpty(TaiwanAPIUrl);
        }
    }
}