namespace LicenseeAgent.ProductSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// CheckExistUserNameViewModel class.
    /// </summary>
    public class CheckExistUserNameViewModel
    {
        [Required(ErrorMessageResourceName = "UsernameMinimumLength", ErrorMessageResourceType = typeof(Las))]
        [StringLength(30, MinimumLength = 6, ErrorMessageResourceName = "UsernameMinimumLength", ErrorMessageResourceType = typeof(Las))]
        [RegularExpression("^[a-zA-Z0-9]{6,30}$", ErrorMessageResourceName = "UsernameMinimumLength", ErrorMessageResourceType = typeof(Las))]
        public string UserName { get; set; }
    }
}