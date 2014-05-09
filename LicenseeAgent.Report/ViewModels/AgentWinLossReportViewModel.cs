// ----------------------------------------------------------------------
// <copyright file="AgentWinLossReportViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Report.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Entities;
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

        public string Product { get; set; }

        public IEnumerable<ProductInfo> ProductList { get; set; }
    }
}
