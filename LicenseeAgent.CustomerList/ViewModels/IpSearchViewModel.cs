// -----------------------------------------------------------------------
// <copyright file="IpSearchViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// IP SearchViewModel class.
    /// </summary>
    public class IpSearchViewModel
    {
        [Required]
        public string IpAddress { get; set; }
    }
}