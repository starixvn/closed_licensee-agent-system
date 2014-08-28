namespace LicenseeAgent.Web.ViewModels
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// SyncMonitoringPostViewModel class.
    /// </summary>
    public class SyncMonitoringPostViewModel
    {
        public int? Action { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed")]
        public string Url { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}