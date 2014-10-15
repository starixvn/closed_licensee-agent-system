namespace LicenseeAgent.ProductSetting.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.ProductSetting.Entities;

    /// <summary>
    /// UpdateCustomerViewModel class.
    /// </summary>
    public class UpdateVirtualSportsCustomerViewModel : IValidatableObject
    {
        [Required]
        public SettingContext SettingContext { get; set; }

        [Required]
        public CustomerSetting VirtualSportsSetting { get; set; }

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
            if (!VirtualSportsSetting.IsValid(ProductCode.Product_5, SettingContext))
            {
                results.Add(new ValidationResult("Invalid Virtual Sports setting"));
                return results;
            }

            return results;
        }
    }
}