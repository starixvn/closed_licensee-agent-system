// --------------------------------------------------------------------------
// <copyright file="InsertCustomerViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.SportsBook;

    /// <summary>
    /// InsertCustomerViewModel class.
    /// </summary>
    public class InsertCustomerViewModel
    {
        [Required]
        public SettingContext SettingContext { get; set; }

        [Required]
        public UpdateDataSportsBookCustomer UpdateDataSportsBookCustomer { get; set; }
    }
}