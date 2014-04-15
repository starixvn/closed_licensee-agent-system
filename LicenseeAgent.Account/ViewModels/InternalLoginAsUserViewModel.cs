// -----------------------------------------------------------------------
// <copyright file="InternalLoginAsUserViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;

    /// <summary>
    /// InternalLoginAsUserViewModel class.
    /// </summary>
    public class InternalLoginAsUserViewModel
    {
        [AllowHtml]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "LicenseeRequired", ErrorMessageResourceType = typeof(Las))]
        public int LicenseeId { get; set; }

        [Required(ErrorMessageResourceName = "CurrencyRequired", ErrorMessageResourceType = typeof(Las))]
        public int CurrencyId { get; set; }
    }
}