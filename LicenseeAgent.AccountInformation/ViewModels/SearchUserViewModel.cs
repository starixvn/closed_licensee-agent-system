// ----------------------------------------------------------------------
// <copyright file="SearchUserViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchUserViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = Constants.MinimumUsernameLength, ErrorMessageResourceName = "NoInfo", ErrorMessageResourceType = typeof(Las))]
        public string UserName { get; set; }
    }
}