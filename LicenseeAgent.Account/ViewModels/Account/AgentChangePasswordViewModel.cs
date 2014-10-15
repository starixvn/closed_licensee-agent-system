namespace LicenseeAgent.Account.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Validations;

    /// <summary>
    /// AgentChangePasswordViewModel class.
    /// </summary>
    public class AgentChangePasswordViewModel
    {
        [AllowHtml]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "CurrentPasswordRequired", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "CurrentPassword", ResourceType = typeof(Las))]
        public string CurrentPassword { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "NewPasswordRequired", ErrorMessageResourceType = typeof(Las))]
        [NotEqual("CurrentPassword", ErrorMessageResourceName = "PasswordMustNotSame", ErrorMessageResourceType = typeof(Las))]
        [AppPasswordRule]
        [StringLength(24, MinimumLength = 8, ErrorMessageResourceName = "PasswordSecurity", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "NewPassword", ResourceType = typeof(Las))]
        public string NewPassword { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Las))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Las))]
        public string ConfirmPassword { get; set; }
    }
}