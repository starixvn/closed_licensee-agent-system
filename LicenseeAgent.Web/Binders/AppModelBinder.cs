// --------------------------------------------------------------------------
// <copyright file="AppModelBinder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Binders
{
    using System.ComponentModel;
    using System.Web;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppModelBinder : DefaultModelBinder
    {
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
    }
}