namespace LicenseeAgent.Web.Mvc
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.WebPages;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// HTML extensions.
    /// </summary>
    public static class HtmlExtensions
    {
        public static HtmlHelper Helper
        {
            get
            {
                return ((WebViewPage)WebPageContext.Current.Page).Html;
            }
        }

        public static IHtmlString Javascript(object value, string name)
        {
            return MvcHtmlString.Create(Formatter.FormatJson(name, value));
        }

        /// <summary>
        /// Bootstrap date picker.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="startDate">The start date.</param>
        /// <returns>
        /// Returns HTML content.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString DatePickerFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            DateTime? startDate)
        {
            var propertyName = ExpressionHelper.GetExpressionText(expression);
            var datePicker = new TagBuilder("div");
            var id = string.Format("dp{0}", propertyName);
            datePicker.GenerateId(id);
            datePicker.AddCssClass("input-append date datepicker");
            if (startDate != null)
            {
                datePicker.Attributes["data-start-date"] = Formatter.FormatDate(startDate.Value);
            }

            // create input
            var inputTag = htmlHelper.TextBoxFor(expression, "{0:MM/dd/yyyy}", new { @class = "input-add-on span2" });

            // create span
            var spanWrapper = new TagBuilder("span");
            spanWrapper.AddCssClass("add-on");
            var iconThWrapper = new TagBuilder("i");
            iconThWrapper.AddCssClass("icon-th");
            spanWrapper.InnerHtml += iconThWrapper.ToString(TagRenderMode.Normal);

            datePicker.InnerHtml += inputTag.ToHtmlString() + spanWrapper.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(datePicker.ToString(TagRenderMode.Normal));
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString DatePickerFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return DatePickerFor(htmlHelper, expression, null);
        }
    }
}