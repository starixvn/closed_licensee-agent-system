namespace LicenseeAgent.Report.ViewModels.AgentOutstanding
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// AgentOutstandingViewModel class.
    /// </summary>
    public class AgentOutstandingViewModel
    {
        [Required]
        public int AgentId { get; set; }

        [Required]
        public bool ShowMembers { get; set; }
    }
}