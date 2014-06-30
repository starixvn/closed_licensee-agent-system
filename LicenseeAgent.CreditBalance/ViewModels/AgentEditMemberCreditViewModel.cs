namespace LicenseeAgent.CreditBalance.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// AgentEditMemberCreditViewModel class.
    /// </summary>
    public class AgentEditMemberCreditViewModel
    {
        [Required]
        public int CustId { get; set; }

        public string MemberName { get; set; }

        public decimal? MaxCreditOutput { get; set; }

        public decimal MinCreditOutput { get; set; }

        public bool IsAccountInfo { get; set; }

        [Required]
        [Display(Name = "Amount", ResourceType = typeof(Las))]
        public decimal GivenCredit { get; set; }
    }
}