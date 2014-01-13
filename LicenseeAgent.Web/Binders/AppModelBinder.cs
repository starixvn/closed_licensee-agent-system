// --------------------------------------------------------------------------
// <copyright file="AppModelBinder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Binders
{
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppModelBinder : DefaultModelBinder
    {
        private static string[] dangerousStrings = new[] { "<", ">", "&#" };

        /// <summary>
        /// Called when the specified property is validating.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates.
        /// The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound.
        /// The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <param name="propertyDescriptor">Describes a property being validated.
        /// The descriptor provides information such as component type, property type, and property value.
        /// It also provides methods to get or set the property value.</param>
        /// <param name="value">The value to set for the property.</param>
        /// <returns>
        /// True if the property is validating; otherwise, false.
        /// </returns>
        protected override bool OnPropertyValidating(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor,
            object value)
        {
            var valid = value is string
                ? ValidateHtmlText(controllerContext, bindingContext, propertyDescriptor, value)
                : true;

            return valid && base.OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, value);
        }

        /// <summary>
        /// Validates the HTML text.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <param name="propertyDescriptor">The property descriptor.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns boolean.</returns>
        protected bool ValidateHtmlText(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor,
            object value)
        {
            var isValidRequest = controllerContext.Controller.ValidateRequest
                                    && bindingContext.PropertyMetadata[propertyDescriptor.Name].RequestValidationEnabled;
            if (isValidRequest && IsDangerousString(value.ToString()))
            {
                bindingContext.ModelState.AddModelError(propertyDescriptor.Name, Las.HtmlDisallowed);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether [is dangerous string] [the specified text].
        /// <![CDATA[ &# and < and > dangerous.]]>
        /// </summary>
        /// <param name="text">The validated text.</param>
        /// <returns>
        /// Returns boolean.
        /// </returns>
        private static bool IsDangerousString(string text)
        {
            return dangerousStrings.Any(s => text.IndexOf(s) >= 0);
        }
    }
}