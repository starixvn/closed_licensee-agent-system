// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CustomerSetting.Entities.Sportsbook;

    public class UpdateCustomerViewModel
    {
        [Required]
        public SettingContext SettingContext { get; set; }

        [Required]
        public UpdateDataSportsbookCustomer UpdateDataSportsbookCustomer { get; set; }
    }
}