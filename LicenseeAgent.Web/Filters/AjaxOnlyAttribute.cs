namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// AjaxOnlyAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsAjaxRequest())
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Only Ajax requests");
            }
        }
    }
}