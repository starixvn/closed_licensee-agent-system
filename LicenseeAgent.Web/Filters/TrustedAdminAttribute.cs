namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// TrustedAdminAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public sealed class TrustedAdminAttribute : BaseActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            if (!currentSession.IsShowMaintenance())
            {
                Unauthorized(filterContext);
            }
        }
    }
}