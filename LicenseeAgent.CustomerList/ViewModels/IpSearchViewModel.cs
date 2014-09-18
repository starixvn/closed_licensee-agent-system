namespace LicenseeAgent.Customer.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// IP SearchViewModel class.
    /// </summary>
    public class IpSearchViewModel
    {
        [Required]
        public string IpAddress { get; set; }
    }
}