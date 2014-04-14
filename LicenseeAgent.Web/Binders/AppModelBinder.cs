// --------------------------------------------------------------------------
// <copyright file="AppModelBinder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Binders
{
    using System.ComponentModel;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;

    public class AppModelBinder : DefaultModelBinder
    {
        private static string[] dangerousStrings = new[] { "<", ">", "&#" };

        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates.</param>
        /// <param name="bindingContext">The context within which the model is bound.</param>
        /// <returns>
        /// The bound object.
        /// </returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                return base.BindModel(controllerContext, bindingContext);
            }
            catch (HttpRequestValidationException)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, Las.HtmlDisallowed);
            }

            // Cast the value provider to an IUnvalidatedValueProvider, which allows to skip validation
            IUnvalidatedValueProvider provider = bindingContext.ValueProvider as IUnvalidatedValueProvider;
            if (provider == null)
            {
                return null;
            }

            // Get the attempted value, skiping the validation
            var result = provider.GetValue(bindingContext.ModelName, skipValidation: true);
            return result.AttemptedValue;
        }

        /// <summary>
        /// Validates the HTML text.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <param name="propertyDescriptor">The property descriptor.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns boolean.</returns>
        protected static bool ValidateHtmlText(
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
        /// Sets the specified property by using the specified controller context, binding context, and property value.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates.</param>
        /// <param name="bindingContext">The context within which the model is bound.</param>
        /// <param name="propertyDescriptor">Describes a property to be set.</param>
        /// <param name="value">The value to set for the property.</param>
        protected override void SetProperty(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor,
            object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                var stringValue = (string)value;
                if (!string.IsNullOrEmpty(stringValue))
                {
                    stringValue = stringValue.Trim();
                }

                value = stringValue;
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }

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