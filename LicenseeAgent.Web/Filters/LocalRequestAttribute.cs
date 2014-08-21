namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// LocalRequestAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class LocalRequestAttribute : BaseActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsLocal)
            {
                NotFound(filterContext);
            }
        }
    }
}