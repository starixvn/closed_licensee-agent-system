// --------------------------------------------------------------------------
// <copyright file="GetSettingForUpdateViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetSettingForUpdateViewModel
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int TargetCustId { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int TargetRoleId { get; set; }
    }
}