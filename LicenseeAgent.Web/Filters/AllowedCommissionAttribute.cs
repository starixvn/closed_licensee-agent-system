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
            var agentInfo = currentSession.AgentInfo;
            if (agentInfo == null || !agentInfo.IsDeposit)
            {
                return false;
            }

            return currentSession.IsAdmin || agentInfo.IsAllowedMemberComm;
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