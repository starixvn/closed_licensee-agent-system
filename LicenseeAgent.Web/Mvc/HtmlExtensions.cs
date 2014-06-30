// --------------------------------------------------------------------------
// <copyright file="HtmlExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

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
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// HTML extensions.
    /// </summary>
    public static class HtmlExtensions
    {
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

        public static IHtmlString Javascript(
            this HtmlHelper htmlHelper,
            object value,
            string name)
        {
            return MvcHtmlString.Create(Formatter.FormatJson(name, value));
        }
    }
}