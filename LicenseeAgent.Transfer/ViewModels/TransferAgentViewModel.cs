namespace LicenseeAgent.Transfer.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TransferAgentViewModel class.
    /// </summary>
    public class TransferAgentViewModel
    {
        [Required]
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        [Required]
        [Display(Name = "Amount", ResourceType = typeof(Las))]
        public decimal Amount { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }
    }
}