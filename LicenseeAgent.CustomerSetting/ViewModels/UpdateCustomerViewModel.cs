// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// UpdateCustomerViewModel class.
    /// </summary>
    public class UpdateCustomerViewModel : IValidatableObject
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
            if (!SportsBookSetting.IsValid(Product.Product_0, SettingContext, true))
            {
                results.Add(new ValidationResult("Invalid SportsBook setting"));
                return results;
            }

            if (NumberGameSetting != null && !NumberGameSetting.IsValid(Product.Product_2, SettingContext, true))
            {
                results.Add(new ValidationResult("Invalid NumberGame setting"));
                return results;
            }

            return results;
        }
    }
}