// --------------------------------------------------------------------------
// <copyright file="AppAuthorizeAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class AppAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether [is internal].
        /// </summary>
        /// <value>
        ///   <c>true</c> if is internal login by CSD/IT, <c>false</c>.
        /// </value>
        public bool IsInternal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether no checking agent to change password and more.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [only authorize]; otherwise, <c>false</c>.
        /// </value>
        public bool OnlyAuthorize { get; set; }

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using
        /// <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (skipAuthorization)
            {
                return;
            }

            // Some controllers which have IsInternal = true must have AppSettings.IsInternal = true
            if (IsInternal && !AppSettings.IsInternal)
            {
                filterContext.ReturnUnauthorized();
                return;
            }

            // Validate current sesssion to kick out
            var currentSession = SessionManager.CurrentSession;
            if (currentSession == null || !currentSession.IsValid)
            {
                filterContext.ReturnUnauthorized();
                return;
            }

            var isAuthorized = AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                filterContext.ReturnAccessDenied();
            }
            else if (!OnlyAuthorize)
            {
                if (currentSession.AgentInfo == null)
                {
                    filterContext.ReturnAccessDenied();
                    return;
                }

                if (!AppSettings.IsInternal && currentSession.IsInternalSignIn)
                {
                    CheckingRequireAgentChangePassword(filterContext);
                }
            }
        }

        /// <summary>
        /// When overridden, provides an entry point for custom authorization checks.
        /// </summary>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns>
        /// True if the user is authorized; otherwise, false.
        /// </returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // don't use base.AuthorizeCore,
            // see: http://stackoverflow.com/questions/3488348/overriding-authorizecore-in-custom-authorize-attribute-results-in-no-suitable-m
            var authorized = httpContext.Request.IsAuthenticated;
            return authorized ? AuthorizeUsers() : false;
        }

        /// <summary>
        /// Authorizes the permission. Only call this function if the httpContext.Request.IsAuthenticated
        /// and session is valid.
        /// </summary>
        /// <returns>True if the user has permission; otherwise, false.</returns>
        /// <exception cref="System.Exception">Please implement here.</exception>
        private bool AuthorizeUsers()
        {
            if (!string.IsNullOrEmpty(Users))
            {
                var currentSession = SessionManager.CurrentSession;
                switch (Users)
                {
                    case Constants.AgentUserOnly:
                        if (currentSession.AgentInfo == null)
                        {
                            return false;
                        }

                        break;

                    case Constants.AdminUserOnly:
                        if (currentSession.AdminInfo == null)
                        {
                            return false;
                        }

                        break;

                    default:
                        throw new Exception("Please implement here.");
                }
            }

            return true;
        }

        /// <summary>
        /// Requires to change password for first login agents and expired password.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        private void CheckingRequireAgentChangePassword(AuthorizationContext filterContext)
        {
            // Only check the agent session.
            var currentSession = SessionManager.CurrentSession;
            var httpContext = filterContext.HttpContext;
            if (filterContext.Result != null)
            {
                return;
            }

            var passwordStatus = PasswordRule.GetPasswordStatus(currentSession.AgentInfo);
            var passwordExpired = passwordStatus == PasswordStatus.Expired || passwordStatus == PasswordStatus.FirstLogin;

            if (passwordExpired)
            {
                filterContext.Result = new RedirectResult(AppURLs.URLs.ChangePassword);
            }
        }
    }
}