// --------------------------------------------------------------------------
// <copyright file="AppModelBinder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Binders
{
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
    }
}