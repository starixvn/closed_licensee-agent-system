namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// AccountTypeAttribute class.
    /// </summary>
    public class AccountTypeAttribute : BaseActionFilterAttribute
    {
        public AccountTypeAttribute(AccountType role)
        {
            Role = role;
        }

        /// <summary>
        /// Gets or sets the account role.
        /// </summary>
        /// <value>
        /// The account role.
        /// </value>
        public AccountType Role { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isAuthorized = AuthorizeRole(filterContext);
            if (!isAuthorized)
            {
                Unauthorized(filterContext);
            }
        }

        /// <summary>
        /// Authorizes the permission. Only call this function if the httpContext.Request.IsAuthenticated
        /// and session is valid.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>Returns the state.</returns>
        /// <exception cref="System.NotSupportedException">Please implement here.</exception>
        /// <exception cref="System.Exception">Please implement here.</exception>
        private bool AuthorizeRole(ActionExecutingContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            var isAuthorized = false;
            switch (Role)
            {
                case AccountType.All:
                    isAuthorized = true;
                    break;

                case AccountType.Admin:
                    isAuthorized = currentSession.IsAdmin;
                    break;

                case AccountType.CSD_IT:
                    isAuthorized = currentSession.IsCSDIT;
                    break;

                case AccountType.Licensee:
                    isAuthorized = currentSession.IsAdmin && currentSession.LoginAccountType == AccountType.Licensee ? true : false;
                    break;

                case AccountType.Agent:
                    isAuthorized = !currentSession.IsAdmin ? true : false;
                    break;

                case AccountType.AgentLevel0:
                    isAuthorized = !currentSession.IsAdmin && currentSession.AgentInfo.IsLevel0 ? true : false;
                    break;

                case AccountType.AgentLevel1:
                    isAuthorized = !currentSession.IsAdmin && currentSession.AgentInfo.IsLevel1 ? true : false;
                    break;

                case AccountType.AgentLeveln:
                    isAuthorized = !currentSession.IsAdmin && currentSession.AgentInfo.IsLastLevel ? true : false;
                    break;

                default:
                    throw new NotSupportedException("Please implement here");
            }

            return isAuthorized;
        }
    }
}