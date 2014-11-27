namespace LicenseeAgent.BetList.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// OutstandingViewModel class.
    /// </summary>
    public class OutstandingViewModel
    {
        [Required]
        public int CustId { get; set; }
    }
}