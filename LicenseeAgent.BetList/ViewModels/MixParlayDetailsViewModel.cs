namespace LicenseeAgent.BetList.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// MixParlayDetailsViewModel class.
    /// </summary>
    public class MixParlayDetailsViewModel
    {
        [Required]
        public long TransId { get; set; }

        [Required]
        public int SportType { get; set; }

        [Required]
        public int BetType { get; set; }
    }
}