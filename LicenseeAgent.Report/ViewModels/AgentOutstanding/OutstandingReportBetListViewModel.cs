namespace LicenseeAgent.Report.ViewModels.AgentOutstanding
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// OutstandingReportBetListViewModel class.
    /// </summary>
    public class OutstandingReportBetListViewModel
    {
        [Required]
        public int CustId { get; set; }
    }
}