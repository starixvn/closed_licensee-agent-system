namespace LicenseeAgent.Web.Filters
{
    using System.Web.Mvc;

    /// <summary>
    /// TrustedAdminAttribute class.
    /// </summary>
    public class TrustedAdminAttribute : BaseActionFilterAttribute
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