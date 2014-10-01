namespace LicenseeAgent.CustomerList.ViewModels.Agent
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// AgentStatusViewModel class.
    /// </summary>
    public class AgentStatusViewModel
    {
        [Required]
        public int AgentId { get; set; }

        [Required]
        public int Status { get; set; }
    }
}