namespace LicenseeAgent.Report.ViewModels
{
    using System.Collections.Generic;
    using Entities;

    /// <summary>
    /// Bet List View Model.
    /// </summary>
    public class OutstandingReportViewModel
    {
        public int AgentId { get; set; }

        public int AgentLevel { get; set; }

        public int NumberOfLevel { get; set; }

        public string BreadCrumb { get; set; }

        public IEnumerable<AgentOutstanding> AgentOutstandings { get; set; }
    }
}