namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Web;

    /// <summary>
    /// Legend Type Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class LegendTypeAttribute : BaseActionFilterAttribute
    {
        public LegendTypeAttribute(LegendType role)
        {
            Role = role;
        }

        public LegendType Role { get; private set; }

        /// <summary>
        /// Authorize Role.
        /// </summary>
        /// <param name="role">Role of User.</param>
        /// <returns>True or False.</returns>
        public static bool AuthorizeRole(LegendType role)
        {
            bool isAuthorized = false;
            ICurrentSession currentSession = SessionManager.CurrentSession;
            if (currentSession.AgentInfo != null)
            {
                switch (role)
                {
                    case LegendType.Credit:
                        isAuthorized = currentSession.AgentInfo.IsCredit;
                        break;
                    case LegendType.Deposit:
                        isAuthorized = currentSession.AgentInfo.IsDeposit;
                        break;
                    default:
                        throw new NotSupportedException("Please implement here");
                }
            }

            return isAuthorized;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!AuthorizeRole())
            {
                Unauthorized(filterContext);
            }
        }

        private bool AuthorizeRole()
        {
            return AuthorizeRole(Role);
        }
    }
}