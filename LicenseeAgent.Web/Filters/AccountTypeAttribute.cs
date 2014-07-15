namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// AccountTypeAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class AccountTypeAttribute : BaseActionFilterAttribute
    {
        public AccountTypeAttribute(AccountType accountType)
        {
            AccountType = accountType;
        }

        /// <summary>
        /// Gets the account type.
        /// </summary>
        /// <value>
        /// The account type.
        /// </value>
        public AccountType AccountType { get; private set; }

        /// <summary>
        /// Authorizes the role.
        /// </summary>
        /// <param name="accountType">The role of User.</param>
        /// <returns>True or False.</returns>
        public static bool Authorize(AccountType accountType)
        {
            var currentSession = SessionManager.CurrentSession;
            var isAuthorized = false;
            switch (accountType)
            {
                case AccountType.All:
                    isAuthorized = true;
                    break;

                case AccountType.Admin:
                    isAuthorized = currentSession.IsAdmin;
                    break;

                case AccountType.CSDIT:
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

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Authorize(AccountType))
            {
                Unauthorized(filterContext);
            }
        }
    }
}