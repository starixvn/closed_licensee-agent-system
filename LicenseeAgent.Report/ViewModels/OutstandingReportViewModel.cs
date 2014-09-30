namespace LicenseeAgent.Report.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.Report.Entities.AgentOutstanding;

    /// <summary>
    /// Bet List View Model.
    /// </summary>
    public class OutstandingReportViewModel
    {
        public int AgentId { get; set; }

        public int AgentLevel { get; set; }

        public int NumberOfLevel { get; set; }

        public string BreadCrumb { get; set; }

        public IList<AgentOutstandingEntity> AgentOutstandings { get; set; }
    }
}