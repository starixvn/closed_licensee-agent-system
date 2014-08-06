namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// AllowedCommissionAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class AllowedCommissionAttribute : BaseActionFilterAttribute
    {
        public static bool IsAllowedCommission()
        {
            var currentSession = SessionManager.CurrentSession;
            if (currentSession.AgentInfo == null)
            {
                return false;
            }

            return SessionManager.CurrentSession.AgentInfo.IsAllowedMemberComm;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!IsAllowedCommission())
            {
                AccessDenied(filterContext);
            }
        }
    }
}
