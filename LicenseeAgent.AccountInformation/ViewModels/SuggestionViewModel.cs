// -----------------------------------------------------------------------
// <copyright file="SuggestionViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuggestionViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string UserName { get; set; }
    }
}