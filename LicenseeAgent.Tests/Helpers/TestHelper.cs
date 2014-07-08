namespace LicenseeAgent.Tests.Helpers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TestExtensions class.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Validate Model.
        /// </summary>
        /// <param name="model"> Model input.</param>
        /// <returns> List validation result.</returns>
        public static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);

            Validator.TryValidateObject(model, context, validationResults, true);

            return validationResults;
        }
    }
}