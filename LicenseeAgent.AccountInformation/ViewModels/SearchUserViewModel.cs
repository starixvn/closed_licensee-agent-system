// ----------------------------------------------------------------------
// <copyright file="SearchUserViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchUserViewModel
    {
        [Required]
        [AllowHtml]
        [StringLength(Constants.MaximumUsernameLength, MinimumLength = Constants.MinimumUsernameLength,
            ErrorMessageResourceName = "NoInfo", ErrorMessageResourceType = typeof(Las))]
        public string UserName { get; set; }
    }
}