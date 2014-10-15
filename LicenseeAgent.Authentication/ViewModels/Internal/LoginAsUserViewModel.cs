namespace LicenseeAgent.Authentication.ViewModels.Internal
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;

    /// <summary>
    /// InternalLoginAsUserViewModel class.
    /// </summary>
    public class LoginAsUserViewModel
    {
        [AllowHtml]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "LicenseeRequired", ErrorMessageResourceType = typeof(Las))]
        public int LicenseeId { get; set; }

        [Required(ErrorMessageResourceName = "CurrencyRequired", ErrorMessageResourceType = typeof(Las))]
        public int CurrencyId { get; set; }
    }
}