// ----------------------------------------------------------------------
// <copyright file="SearchUserViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchUserViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string UserName { get; set; }
    }
}