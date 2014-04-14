// -----------------------------------------------------------------------
// <copyright file="AgentBreadcrumModelBinder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web.Binders
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.ViewModels;

    public class AgentBreadcrumModelBinder : IModelBinder
    {
        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The bound value.
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var agentId = request.Form.Get("AgentId");
            if (string.IsNullOrEmpty(agentId))
            {
                bindingContext.ModelState.AddModelError("AgentId", "Required.");
                return null;
            }

            var agentLevel = request.Form.Get("AgentLevel");
            if (string.IsNullOrEmpty(agentLevel))
            {
                bindingContext.ModelState.AddModelError("AgentLevel", "Required.");
                return null;
            }

            var agentName = request.Form.Get("AgentName");
            if (string.IsNullOrEmpty(agentName))
            {
                bindingContext.ModelState.AddModelError("AgentName", "Required.");
                return null;
            }

            var breadcrumb = request.Form.Get("Breadcrumb");
            if (breadcrumb == null)
            {
                bindingContext.ModelState.AddModelError("Breadcrumb", "Required.");
                return null;
            }

            return new AgentBreadcrumViewModel
            {
                AgentId = Convert.ToInt32(agentId),
                AgentName = agentName,
                AgentLevel = Convert.ToInt16(agentLevel),
                Breadcrumb = Breadcrumb.FromString(breadcrumb)
            };
        }
    }
}