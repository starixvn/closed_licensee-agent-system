// --------------------------------------------------------------------------
// <copyright file="AppModelBinder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Binders
{
    using System;
    using System.ComponentModel;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppModelBinder : DefaultModelBinder
    {
        private static char[] dangerousChars = new char[] { '<', '&' };

        private IAuthorizeService _authorizeService;

        public AppModelBinder()
        {
            _authorizeService = DependencyResolver.Current.GetService<IAuthorizeService>();
        }

        /// <summary>
        /// Called when the model is updated.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates.</param>
        /// <param name="bindingContext">The context within which the model is bound.</param>
        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);
            var downlineContext = bindingContext.Model as DownlineContext;
            if (downlineContext != null)
            {
                if (downlineContext.TargetCustId == 0)
                {
                    return;
                }

                var criteria = new CheckDownlineBelongCriteria
                {
                    CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                    TargetCustId = downlineContext.TargetCustId,
                    Type = downlineContext.TargetRoleId == (int)Role.Member ? (int)DownlineRole.Member : (int)DownlineRole.Agent
                };

                ValidateDownlineBelong(controllerContext, criteria);
            }
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
            var isValid = true;
            if (value is string)
            {
                isValid = ValidateHtmlText(controllerContext, bindingContext, propertyDescriptor, value);
            }
            else if (value is int)
            {
                isValid = ValidateDownlineBelong(controllerContext, bindingContext, propertyDescriptor, value);
            }
            else if (value is int?)
            {
                isValid = ValidateDownlineBelong(controllerContext, bindingContext, propertyDescriptor, value);
            }

            if (!isValid)
            {
                return false;
            }

            return base.OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, value);
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
        /// Validates the down-line belong.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <param name="propertyDescriptor">The property descriptor.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns boolean.</returns>
        protected bool ValidateDownlineBelong(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor,
            object value)
        {
            if (value == null)
            {
                return true;
            }

            var propertyName = propertyDescriptor.Name;
            var targetCustId = Convert.ToInt32(value);
            var isValidateDownline = false;
            var downlineRole = DownlineRole.Agent;
            if (propertyName.Equals("agentId", StringComparison.OrdinalIgnoreCase))
            {
                isValidateDownline = true;
            }
            else if (propertyName.Equals("memberId", StringComparison.OrdinalIgnoreCase))
            {
                isValidateDownline = true;
                downlineRole = DownlineRole.Member;
            }

            if (isValidateDownline)
            {
                var criteria = new CheckDownlineBelongCriteria
                {
                    CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                    TargetCustId = targetCustId,
                    Type = (int)downlineRole
                };

                return ValidateDownlineBelong(controllerContext, criteria);
            }

            return true;
        }

        /// <summary>
        /// Validates the down-line belong.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Returns boolean.
        /// </returns>
        protected bool ValidateDownlineBelong(
            ControllerContext controllerContext,
            CheckDownlineBelongCriteria criteria)
        {
            _authorizeService.CheckDownlineBelong(criteria);
            if (!criteria.IsBelongTo)
            {
                controllerContext.ReturnAccessDenied();
                return false;
            }
            else
            {
                var controllerType = controllerContext.Controller.GetType();
                var controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
                var actionName = controllerContext.RouteData.GetRequiredString("action");
                var actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);
                var isValidateDirectDownline = actionDescriptor.IsDefined(typeof(DirectDownlineAttribute), true)
                || controllerDescriptor.IsDefined(typeof(DirectDownlineAttribute), true);
                if (isValidateDirectDownline && !_authorizeService.ValidateDirectDownline(criteria))
                {
                    controllerContext.ReturnAccessDenied();
                    return false;
                }
            }

            return true;
        }

        private static bool IsAtoZ(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        /// <summary>
        /// Determines whether [is dangerous string] [the specified text].
        /// <![CDATA[ &# and <# are dangerous.]]>
        /// <![CDATA[ <(A-Z) and <! and </ and <? are dangerous.]]>
        /// </summary>
        /// <param name="text">The validated text.</param>
        /// <returns>
        /// Returns boolean.
        /// </returns>
        private static bool IsDangerousString(string text)
        {
            var startIndex = 0;
            var textLength = text.Length;
            while (startIndex <= textLength)
            {
                // Return false if the text does not contain any dangerous chars.
                var index = text.IndexOfAny(dangerousChars, startIndex);
                if (index < 0 || index >= textLength - 1)
                {
                    return false;
                }

                var currentChar = text[index];
                var nextChar = text[index + 1];

                // &# and <# is dangerous
                if (nextChar == '#')
                {
                    return true;
                }
                else if (currentChar == '<')
                {
                    var isNextCharDangerous = IsAtoZ(nextChar)
                        || nextChar == '!'
                         || nextChar == '/' || nextChar == '?';
                    if (isNextCharDangerous)
                    {
                        return true;
                    }
                }

                startIndex = index + 1;
            }

            return false;
        }
    }
}