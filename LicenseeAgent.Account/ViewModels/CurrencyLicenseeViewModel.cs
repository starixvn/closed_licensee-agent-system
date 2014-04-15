// -----------------------------------------------------------------------
// <copyright file="CurrencyLicenseeViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// CurrencyLicenseeViewModel class.
    /// </summary>
    public class CurrencyLicenseeViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int LicenseeId { get; set; }
    }
}