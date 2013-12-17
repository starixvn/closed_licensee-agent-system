// --------------------------------------------------------------------------
// <copyright file="ValidateSessionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Account.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ValidateSessionViewModel
    {
        [Required(ErrorMessageResourceName = "LicenseeRequired", ErrorMessageResourceType = typeof(Las))]
        public int LicenseeId { get; set; }
    }
}