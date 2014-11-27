namespace LicenseeAgent.BetList.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// SystemParlayDetailsViewModel class.
    /// </summary>
    public class SystemParlayDetailsViewModel
    {
        [Required]
        public long RefNo { get; set; }
    }
}