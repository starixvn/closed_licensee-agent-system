namespace LicenseeAgent.Home.ViewModels.TopMenu
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// CurrencyLicenseeViewModel class.
    /// </summary>
    public class CurrencyLicenseeViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int LicenseeId { get; set; }
    }
}