// --------------------------------------------------------------------------
// <copyright file="InsertCustomerViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// InsertCustomerViewModel class.
    /// </summary>
    public class InsertCustomerViewModel : IValidatableObject
    {
        [Required]
        public SettingContext SettingContext { get; set; }

        [Required]
        public CustomerSetting SportsBookSetting { get; set; }

        public CustomerSetting NumberGameSetting { get; set; }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (SportsBookSetting.CommissionSetting == null
                || SportsBookSetting.CommissionSetting.CommissionItems == null
                || SportsBookSetting.CommissionSetting.CommissionItems.Count == 0
                || SportsBookSetting.BettingLimitSetting == null
                || SportsBookSetting.BettingLimitSetting.Count == 0
                || SportsBookSetting.PositionTakingSetting == null
                || SportsBookSetting.PositionTakingSetting.Count == 0)
            {
                results.Add(new ValidationResult("Invalid SportsBook setting"));
            }

            if (NumberGameSetting != null)
            {
                if (NumberGameSetting.CommissionSetting == null
                    || NumberGameSetting.CommissionSetting.CommissionItems == null
                    || NumberGameSetting.CommissionSetting.CommissionItems.Count == 0
                    || SportsBookSetting.BettingLimitSetting == null
                    || SportsBookSetting.BettingLimitSetting.Count == 0
                    || SportsBookSetting.PositionTakingSetting == null
                    || SportsBookSetting.PositionTakingSetting.Count == 0)
                {
                    results.Add(new ValidationResult("Invalid NumberGame setting"));
                }
            }

            return results;
        }
    }
}