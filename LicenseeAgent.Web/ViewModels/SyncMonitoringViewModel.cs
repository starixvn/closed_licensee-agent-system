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
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Data's Synchronization Monitoring View Model.
    /// </summary>
    public class SyncMonitoringViewModel
    {
        public SyncMonitoringViewModel()
        {
            Actions = new List<SelectListItem>();
        }

        public string TaiwanAPIUrl { get; set; }

        public IList<SelectListItem> Actions { get; set; }
    }
}
