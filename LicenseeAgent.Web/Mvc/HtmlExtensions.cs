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
        public static IHtmlString Javascript(
            object value,
            string name)
        {
            return MvcHtmlString.Create(Formatter.FormatJson(name, value));
        }
    }
}