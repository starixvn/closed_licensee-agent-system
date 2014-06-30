namespace LicenseeAgent.AccountInformation.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// SuggestionViewModel class.
    /// </summary>
    public class SuggestionViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [AllowHtml]
        public string UserName { get; set; }
    }
}