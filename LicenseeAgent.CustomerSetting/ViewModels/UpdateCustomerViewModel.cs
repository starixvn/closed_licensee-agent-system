// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CustomerSetting.Entities.Sportsbook;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateCustomerViewModel
    {
        [Required]
        public SettingContext SettingContext { get; set; }

        [Required]
        public UpdateDataSportsbookCustomer UpdateDataSportsbookCustomer { get; set; }
    }
}