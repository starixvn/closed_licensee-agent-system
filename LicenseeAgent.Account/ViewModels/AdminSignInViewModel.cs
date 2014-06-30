namespace LicenseeAgent.Account.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;

    /// <summary>
    /// AdminSignInViewModel class.
    /// </summary>
    public class AdminSignInViewModel
    {
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "Username", ResourceType = typeof(Las))]
        public string Username { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Las))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Las))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "LanguageRequired", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "Language", ResourceType = typeof(Las))]
        public string Language { get; set; }
    }
}