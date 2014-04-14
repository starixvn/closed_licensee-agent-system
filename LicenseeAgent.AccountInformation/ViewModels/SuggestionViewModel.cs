// -----------------------------------------------------------------------
// <copyright file="SuggestionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class SuggestionViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [AllowHtml]
        public string UserName { get; set; }
    }
}