namespace LicenseeAgent.Account.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Agent Sign In Page Model.
    /// </summary>
    public class AgentSignInViewModel
    {
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "Username", ResourceType = typeof(Las))]
        public string Username { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Las))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Las))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "CaptchaRequired", ErrorMessageResourceType = typeof(Las))]
        [StringLength(6, MinimumLength = 6, ErrorMessageResourceName = "CaptchaLength", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "Captcha", ResourceType = typeof(Las))]
        public string Captcha { get; set; }

        [Required(ErrorMessageResourceName = "LanguageRequired", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "Language", ResourceType = typeof(Las))]
        public string Language { get; set; }
    }
}