namespace LicenseeAgent.ProductSetting.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.ProductSetting.Entities;

    /// <summary>
    /// UpdateCustomerViewModel class.
    /// </summary>
    public class UpdateCustomerViewModel : IValidatableObject
    {
        [Required]
        public SettingContext SettingContext { get; set; }

        [Required]
        public CustomerSetting CustomerSetting { get; set; }

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

            if (!CustomerSetting.IsValid(SettingContext.ProductId, SettingContext))
            {
                results.Add(new ValidationResult("Invalid Customer setting"));
                return results;
            }

            return results;
        }
    }
}
