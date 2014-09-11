namespace LicenseeAgent.Home.ViewModels.AccountInformation
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// SearchUserViewModel class.
    /// </summary>
    public class SearchUserViewModel
    {
        [Required]
        [AllowHtml]
        [StringLength(Constants.MaximumUsernameLength, MinimumLength = Constants.MinimumUsernameLength,
            ErrorMessageResourceName = "SearchUsernameMinLength", ErrorMessageResourceType = typeof(Las))]
        public string UserName { get; set; }
    }
}