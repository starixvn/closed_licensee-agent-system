namespace LicenseeAgent.Report.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Report.Enumerations;
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
            DrawFilter = DrawFilter.Normal;
        }

        public int AgentId { get; set; }

        public int AgentLevel { get; set; }

        public string UserName { get; set; }

        public string BreadCrumb { get; set; }

        [Required]
        [Display(Name = "DateRange", ResourceType = typeof(Las))]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To", ResourceType = typeof(Las))]
        public DateTime ToDate { get; set; }

        [Required]
        [Display(Name = "DrawFilter", ResourceType = typeof(Las))]
        public DrawFilter DrawFilter { get; set; }

        public string Product { get; set; }

        public bool IsDeposit { get; set; }

        public IList<ProductInfo> ProductList { get; set; }
    }
}