// ----------------------------------------------------------------------
// <copyright file="SyncMonitoringViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/04/22">Created</change>
//   <change who="" date=""></change>
// </history>

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
