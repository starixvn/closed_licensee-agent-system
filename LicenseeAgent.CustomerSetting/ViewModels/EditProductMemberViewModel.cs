// -----------------------------------------------------------------------
// <copyright file="EditProductMemberViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditProductMemberViewModel
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public Product ProductId { get; set; }
    }
}