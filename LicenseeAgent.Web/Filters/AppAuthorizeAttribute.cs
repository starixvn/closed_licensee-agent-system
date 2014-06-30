namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Web.Criteria;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// Application authorize attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public sealed class AppAuthorizeAttribute : AuthorizeAttribute
    {
        private IAuthorizeService _authorizeService;

        public AppAuthorizeAttribute()
            : this(null)
        {
        }

        public AppAuthorizeAttribute(IAuthorizeService authorizeService)
        {
            _authorizeService = authorizeService ?? new AuthorizeService();
            IsValidateAgent = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether validation is applied on agent information.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [only authorize]; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidateAgent { get; set; }

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using
        /// <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (skipAuthorization)
            {
                return;
            }

            base.OnAuthorization(filterContext);

            if (!filterContext.HttpContext.Response.IsRequestBeingRedirected && filterContext.Result == null)
            {
                AuthorizeSession(filterContext);
            }

            if (!filterContext.HttpContext.Response.IsRequestBeingRedirected && filterContext.Result == null)
            {
                AuthorizeCurrencyPermission(filterContext);
            }

            if (!filterContext.HttpContext.Response.IsRequestBeingRedirected && filterContext.Result == null)
            {
                AuthorizeAgent(filterContext);
            }
        }

        private static void AuthorizeSession(AuthorizationContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            if (currentSession == null || !currentSession.IsValid)
            {
                filterContext.ReturnUnauthorized();
            }
        }

        /// <summary>
        /// Validates the agent information.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        private void AuthorizeAgent(AuthorizationContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            if (currentSession == null || !IsValidateAgent)
            {
                return;
            }

            if (currentSession.LoginAccountType == AccountType.Agent)
            {
                var passwordStatus = PasswordRule.GetPasswordStatus(currentSession.AgentInfo);
                var passwordExpired = passwordStatus == PasswordStatus.Expired || passwordStatus == PasswordStatus.FirstLogin;
                if (passwordExpired)
                {
                    filterContext.Result = new RedirectResult(AppURLs.URLs.ChangePassword);
                }
            }
        }

        /// <summary>
        /// Validates the currency permission.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        private void AuthorizeCurrencyPermission(AuthorizationContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            if (currentSession == null || currentSession.LoginAccountType == AccountType.Agent || currentSession.AgentInfo == null || !IsValidateAgent)
            {
                return;
            }

            var criteria = new CheckCurrencyPermissionCriteria
            {
                AdminId = currentSession.LoginUserId,
                LicenseeId = currentSession.AgentInfo.LicenseeId,
                CurrencyId = currentSession.AgentInfo.CurrencyId
            };

            _authorizeService.CheckCurrencyPermission(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                filterContext.ReturnCurrencyPermissionDenied();
            }
        }
    }
}