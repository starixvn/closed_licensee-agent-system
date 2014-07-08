namespace LicenseeAgent.Tests.Helpers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// ControllerExtensions class.
    /// </summary>
    public static class MvcExtensions
    {
        /// <summary>
        /// Validates the model.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="model">The model.</param>
        /// <returns>Returns status of model is valid or not.</returns>
        public static bool ValidateModel(this Controller controller, object model)
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            return validationResults.Count == 0;
        }

        public static string GetErrorMessage(this ModelStateDictionary dictionary, string key)
        {
            return dictionary[key].Errors.First().ErrorMessage;
        }

        public static string GetErrorMessage(this ModelStateDictionary dictionary)
        {
            return GetErrorMessage(dictionary, string.Empty);
        }
    }
}