// --------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Fanex.Common;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Extensions
    {
        public static string GetActionName(this HttpContextBase httpContext)
        {
            return httpContext.Request.RequestContext.RouteData.GetRequiredString("action");
        }

        public static string GetControllerName(this HttpContextBase httpContext)
        {
            return httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
        }

        public static string GetAreaName(this HttpContextBase httpContext)
        {
            return httpContext.Request.RequestContext.RouteData.Values["area"] as string;
        }

        public static int GetInt32Parameter(this ActionExecutingContext context, string name, int defaultValue = -1)
        {
            var actionParameters = context.ActionParameters;
            if (actionParameters.ContainsKey(name))
            {
                return Converter.ToInt(actionParameters[name], defaultValue);
            }

            return defaultValue;
        }

        public static bool IsAjaxRequest(this ControllerContext context)
        {
            return IsAjaxRequestInternal(context.HttpContext.Request);
        }

        public static bool IsAjaxRequest(this ActionExecutingContext context)
        {
            return IsAjaxRequestInternal(context.HttpContext.Request);
        }

        public static bool IsAjaxRequest(this HttpRequest httpRequest)
        {
            var httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
            return IsAjaxRequestInternal(httpContextWrapper.Request);
        }

        public static bool IsAjaxRequest(this AuthorizationContext context)
        {
            return IsAjaxRequestInternal(context.HttpContext.Request);
        }

        public static void WriteJson(this HttpResponse httpResponse, Result jsonResult)
        {
            httpResponse.AddHeader("content-type", "application/json");
            httpResponse.Write(Formatter.FormatJson(jsonResult));
        }

        /// <summary>
        /// Create a textbox with MAXLENGTH property.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Returns a MVC HTML String.</returns>
        public static MvcHtmlString LasTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            var member = expression.Body as MemberExpression;
            var stringLength = member.Member
                .GetCustomAttributes(typeof(StringLengthAttribute), false)
                .FirstOrDefault() as StringLengthAttribute;

            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);
            if (stringLength != null)
            {
                attributes.Add("maxlength", stringLength.MaximumLength);
            }

            return htmlHelper.TextBoxFor(expression, attributes);
        }

        private static bool IsAjaxRequestInternal(HttpRequestBase httpRequest)
        {
            var ajaxRequest = httpRequest.IsAjaxRequest();
            var jsonRequest = httpRequest.AcceptTypes.Contains("application/json");

            return ajaxRequest || jsonRequest;
        }
    }
}