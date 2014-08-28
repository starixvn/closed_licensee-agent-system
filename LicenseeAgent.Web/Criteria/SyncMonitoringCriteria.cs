namespace LicenseeAgent.Web.Criteria
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data.Repository;

    /// <summary>
    /// SyncMonitoringCriteria class.
    /// </summary>
    public class SyncMonitoringCriteria : CriteriaBase
    {
        public int ActionId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Verified")]
        public string TaiwanAPIUrl { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SyncMonitoring";
        }

        public override bool IsValid()
        {
            return ActionId >= 0;
        }
    }
}