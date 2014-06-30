namespace LicenseeAgent.CreditBalance.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// EditAgentCreditViewModel class.
    /// </summary>
    public class EditAgentCreditViewModel
    {
        [Required]
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public int AgentLevel { get; set; }

        public decimal? MaxCreditOutput { get; set; }

        public decimal MinCreditOutput { get; set; }

        public bool IsAccountInfo { get; set; }

        [Required]
        [Display(Name = "Amount", ResourceType = typeof(Las))]
        public decimal GivenCredit { get; set; }
    }
}