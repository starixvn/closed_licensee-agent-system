// --------------------------------------------------------------------------
// <copyright file="GetSettingForUpdateViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// GetSettingForUpdateViewModel class.
    /// </summary>
    public class GetSettingForUpdateViewModel
    {
        [Required]
        public Product ProductId { get; set; }

        [Required]
        public int CustId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int TargetCustId { get; set; }

        [Required]
        public int TargetRoleId { get; set; }

        [Required]
        public int FormId { get; set; }
    }
}