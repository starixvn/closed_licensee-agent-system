namespace LicenseeAgent.Web.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Data's Synchronization Monitoring View Model.
    /// </summary>
    public class SyncMonitoringViewModel
    {
        public SyncMonitoringViewModel()
        {
            APIUrls = new List<SelectListItem>();
        }

        public IList<SelectListItem> APIUrls { get; set; }
    }
}