// -----------------------------------------------------------------------
// <copyright file="CustomerSearchAutoCompleteViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// CustomerSearchAutoCompleteViewModel class.
    /// </summary>
    public class CustomerSearchAutoCompleteViewModel
    {
        [Required]
        public int AgentId { get; set; }

        [Required]
        public int AgentLevel { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [AllowHtml]
        public string UserName { get; set; }
    }
}