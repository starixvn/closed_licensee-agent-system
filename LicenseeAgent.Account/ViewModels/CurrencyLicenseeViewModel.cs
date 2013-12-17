// -----------------------------------------------------------------------
// <copyright file="CurrencyLicenseeViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CurrencyLicenseeViewModel
    {
        [Required(ErrorMessageResourceName = "LicenseeRequired", ErrorMessageResourceType = typeof(Las))]
        public int LicenseeId { get; set; }

        public bool IsValid()
        {
            if (LicenseeId > 0)
            {
                return true;
            }

            return false;
        }
    }
}
