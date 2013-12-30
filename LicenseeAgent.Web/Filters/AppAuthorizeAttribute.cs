// --------------------------------------------------------------------------
// <copyright file="AppAuthorizeAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

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
        /// Gets or sets a value indicating whether site is only allowed by admin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is internal login by CSD/IT, <c>false</c>.
        /// </value>
        public bool OnlyAdminSite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether validation is applied on agent information.
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
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (skipAuthorization)
            {
                return;
            }

            if (OnlyAdminSite && !AppSettings.IsAdminSite)
            {
                filterContext.ReturnUnauthorized();
                return;
            }

            var currentSession = SessionManager.CurrentSession;
            if (currentSession == null || !currentSession.IsValid)
            {
                filterContext.ReturnUnauthorized();
                return;
            }

            var isAuthorized = AuthorizeCore(filterContext.HttpContext);
            if (!isAuthorized)
            {
                filterContext.ReturnAccessDenied();
                return;
            }

            if (OnlyAuthorize)
            {
                return;
            }

            if (!ValidateAgent(filterContext))
            {
                return;
            }
        }

        /// <summary>
        /// Validates the agent information.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>Returns true if agent is valid.</returns>
        protected bool ValidateAgent(AuthorizationContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            if (currentSession.AgentInfo == null)
            {
                filterContext.ReturnAccessDenied();
                return false;
            }

            if (!AppSettings.IsAdminSite && currentSession.IsDirectSignIn)
            {
                var passwordStatus = PasswordRule.GetPasswordStatus(currentSession.AgentInfo);
                var passwordExpired = passwordStatus == PasswordStatus.Expired || passwordStatus == PasswordStatus.FirstLogin;
                if (passwordExpired)
                {
                    filterContext.Result = new RedirectResult(AppURLs.URLs.ChangePassword);
                    return false;
                }
            }

            return true;
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
    }
}