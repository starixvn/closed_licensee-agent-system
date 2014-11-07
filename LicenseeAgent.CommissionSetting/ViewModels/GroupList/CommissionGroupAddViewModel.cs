namespace LicenseeAgent.CommissionSetting.ViewModels.GroupList
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using LicenseeAgent.CommissionSetting.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;

    /// <summary>
    /// CommissionGroupAddViewModell class.
    /// </summary>
    public class CommissionGroupAddViewModel : IValidatableObject
    {
        [Required(ErrorMessageResourceName = "GroupNameIsEmpty", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "GroupName", ResourceType = typeof(Las))]
        public string GroupName { get; set; }

        [Required]
        [Display(Name = "SetAsDefaultGroup", ResourceType = typeof(Las))]
        public bool IsDefault { get; set; }

        [Required]
        public IList<ProductCommission> ProductCommissions { get; set; }

        /// <summary>
        /// Gets the product commission.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="productCode">The product code.</param>
        /// <returns>Returns product commission.</returns>
        public decimal GetProductCommission(IList<ProductCommissionColumn> columns, ProductCode productCode)
        {
            if (ProductCommissions != null && columns.Where(x => x.ProductId == (int)productCode).Any())
            {
                var productCommission = ProductCommissions.Where(x => x.ProductId == (int)productCode).FirstOrDefault();
                if (productCommission != null)
                {
                    return productCommission.Commission;
                }
            }

            return 0;
        }

        public bool HasProductCommission(ProductCode productCode)
        {
            return ProductCommissions != null && ProductCommissions.Where(x => x.ProductId == (int)productCode).Any();
        }

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
            if (ProductCommissions != null)
            {
                foreach (var productCommission in ProductCommissions)
                {
                    if (productCommission.Commission < 0 || productCommission.Commission > 0.15m)
                    {
                        var message = string.Format(
                            "Invalid commission value ({0}): {1}",
                            DataRefWrapper.GetProductName(productCommission.ProductId),
                            productCommission.Commission);
                        results.Add(new ValidationResult(message));
                        break;
                    }
                }
            }

            return results;
        }
    }
}