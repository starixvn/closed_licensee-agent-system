namespace LicenseeAgent.Customer.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Validations;

    /// <summary>
    /// ResetPasswordViewModel class.
    /// </summary>
    public class ResetPasswordViewModel
    {
        public int CustId { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Las))]
        [AppPasswordRule]
        [StringLength(24, MinimumLength = 8, ErrorMessageResourceName = "PasswordSecurity", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "Password", ResourceType = typeof(Las))]
        public string Password { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Las))]
        [Compare("Password", ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Las))]
        public string ConfirmPassword { get; set; }
    }
}