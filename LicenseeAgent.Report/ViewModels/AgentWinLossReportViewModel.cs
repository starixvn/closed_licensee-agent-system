// ----------------------------------------------------------------------
// <copyright file="AgentWinLossReportViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/17">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Agent WinLoss Report View Model.
    /// </summary>
    public class AgentWinLossReportViewModel
    {
        public AgentWinLossReportViewModel()
        {
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            DrawFilter = 0;
        }

        public int AgentId { get; set; }

        public int AgentLevel { get; set; }

        public string Username { get; set; }
        
        public string BreadCrumb { get; set; }

        [Required]
        [Display(Name = "DateRange", ResourceType = typeof(Las))]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To", ResourceType = typeof(Las))]
        public DateTime ToDate { get; set; }

        [Display(Name = "DrawFilter", ResourceType = typeof(Las))]
        public short DrawFilter { get; set; }
    }
}
