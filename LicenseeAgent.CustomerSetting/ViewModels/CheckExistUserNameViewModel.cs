// --------------------------------------------------------------------------
// <copyright file="CheckExistUserNameViewModel.cs" company="Nexcel Solutions Vietnam">
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
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckExistUserNameViewModel
    {
        [Required(ErrorMessageResourceName = "UsernameMinimumLength", ErrorMessageResourceType = typeof(Las))]
        [StringLength(50, MinimumLength = 6, ErrorMessageResourceName = "UsernameMinimumLength", ErrorMessageResourceType = typeof(Las))]
        public string UserName { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int TargetRoleId { get; set; }
    }
}