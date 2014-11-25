namespace LicenseeAgent.Report.ViewModels.Statement
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Statement Report View Model.
    /// </summary>
    public class StatementDetailReportViewModel
    {
        public int AgentId { get; set; }

        public int AgentLevel { get; set; }

        [Display(Name = "DateRange", ResourceType = typeof(Las))]
        public DateTime FromDate { get; set; }

        [Display(Name = "To", ResourceType = typeof(Las))]
        public DateTime ToDate { get; set; }

        [AllowHtml]
        [Display(Name = "Username", ResourceType = typeof(Las))]
        public string Username { get; set; }

        public string Product { get; set; }

        public bool HideUsername { get; set; }

        public bool IsMember { get; set; }

        public bool ShowBackButton { get; set; }
    }
}