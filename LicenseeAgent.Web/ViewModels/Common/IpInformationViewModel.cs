namespace LicenseeAgent.Web.ViewModels.Common
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// IP InformationViewModel class.
    /// </summary>
    public class IpInformationViewModel
    {
        [Required]
        public string IpAddress { get; set; }

        public string Country { get; set; }

        public string City { get; set; }
    }
}