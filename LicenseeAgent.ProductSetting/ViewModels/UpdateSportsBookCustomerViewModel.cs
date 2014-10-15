namespace LicenseeAgent.ProductSetting.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.ProductSetting.Entities;

    /// <summary>
    /// UpdateCustomerViewModel class.
    /// </summary>
    public class UpdateSportsBookCustomerViewModel : IValidatableObject
    {
        [Required]
        public SettingContext SettingContext { get; set; }

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
            if (SportsBookSetting != null && !SportsBookSetting.IsValid(ProductCode.Product_0, SettingContext))
            {
                results.Add(new ValidationResult("Invalid SportsBook setting"));
                return results;
            }

            if (NumberGameSetting != null && !NumberGameSetting.IsValid(ProductCode.Product_2, SettingContext))
            {
                results.Add(new ValidationResult("Invalid NumberGame setting"));
                return results;
            }

            return results;
        }
    }
}