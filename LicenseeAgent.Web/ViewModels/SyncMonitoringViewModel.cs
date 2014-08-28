namespace LicenseeAgent.Web.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Data's Synchronization Monitoring View Model.
    /// </summary>
    public class SyncMonitoringViewModel
    {
        public IList<SyncMonitoringAction> Actions { get; set; }

        public IList<string> Urls { get; set; }
    }
}