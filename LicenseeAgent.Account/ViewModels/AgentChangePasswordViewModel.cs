// --------------------------------------------------------------------------
// <copyright file="AgentChangePasswordViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Account.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Validations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentChangePasswordViewModel
    {
        [AllowHtml]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "CurrentPasswordRequired", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "CurrentPassword", ResourceType = typeof(Las))]
        public string CurrentPassword { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "NewPasswordRequired", ErrorMessageResourceType = typeof(Las))]
        [NotEqual("CurrentPassword", ErrorMessageResourceName = "PasswordMustNotSame", ErrorMessageResourceType = typeof(Las))]
        [AppPasswordRule]
        [StringLength(24, MinimumLength = 8, ErrorMessageResourceName = "PasswordSecurity", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "NewPassword", ResourceType = typeof(Las))]
        public string NewPassword { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Las))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Las))]
        public string ConfirmPassword { get; set; }
    }
}