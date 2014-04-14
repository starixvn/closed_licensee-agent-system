// --------------------------------------------------------------------------
// <copyright file="ViewAsLicenseeViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Account.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    public class ViewAsLicenseeViewModel
    {
        [Required(ErrorMessageResourceName = "LicenseeRequired", ErrorMessageResourceType = typeof(Las))]
        public int LicenseeId { get; set; }

        [Required(ErrorMessageResourceName = "CurrencyRequired", ErrorMessageResourceType = typeof(Las))]
        public int CurrencyId { get; set; }
    }
}