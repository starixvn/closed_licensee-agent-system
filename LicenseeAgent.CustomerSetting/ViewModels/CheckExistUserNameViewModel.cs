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
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    public class CheckExistUserNameViewModel
    {
        [Required(ErrorMessageResourceName = "UsernameMinimumLength", ErrorMessageResourceType = typeof(Las))]
        [StringLength(30, MinimumLength = 6, ErrorMessageResourceName = "UsernameMinimumLength", ErrorMessageResourceType = typeof(Las))]
        [RegularExpression("^[a-zA-Z0-9]{6,30}$", ErrorMessageResourceName = "UsernameMinimumLength", ErrorMessageResourceType = typeof(Las))]
        public string UserName { get; set; }
    }
}