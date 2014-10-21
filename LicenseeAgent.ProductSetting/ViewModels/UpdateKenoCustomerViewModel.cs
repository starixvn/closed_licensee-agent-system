namespace LicenseeAgent.ProductSetting.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.ProductSetting.Entities;

    /// <summary>
    /// UpdateKenoCustomerViewModel class.
    /// </summary>
    public class UpdateKenoCustomerViewModel : IValidatableObject
    {
        [Required]
        public SettingContext SettingContext { get; set; }

        [Required]
        public CustomerSetting KenoSetting { get; set; }

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
            if (!KenoSetting.IsValid(ProductCode.Product_8, SettingContext))
            {
                results.Add(new ValidationResult("Invalid Keno setting"));
                return results;
            }

            return results;
        }
    }
}
