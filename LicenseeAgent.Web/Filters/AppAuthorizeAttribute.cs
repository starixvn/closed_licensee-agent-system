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
        public AppAuthorizeAttribute()
        {
            IsValidateAgent = true;
            Role = AccountType.All;
        }

        /// <summary>
        /// Gets or sets a value indicating whether validation is applied on agent information.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [only authorize]; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidateAgent { get; set; }

        public AccountType Role { get; set; }

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

            if (IsValidateAgent && !ValidateAgent(filterContext))
            {
                return;
            }
        }

        /// <summary>
        /// Validates the agent information.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>Returns true if agent is valid.</returns>
        protected static bool ValidateAgent(AuthorizationContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            if (currentSession.AgentInfo == null)
            {
                filterContext.ReturnAccessDenied();
                return false;
            }

            if (!currentSession.IsAdmin && currentSession.IsDirectSignIn)
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
            return authorized ? AuthorizeRole() : false;
        }

        /// <summary>
        /// Authorizes the permission. Only call this function if the httpContext.Request.IsAuthenticated
        /// and session is valid.
        /// </summary>
        /// <returns>True if the user has permission; otherwise, false.</returns>
        /// <exception cref="System.Exception">Please implement here.</exception>
        private bool AuthorizeRole()
        {
            var currentSession = SessionManager.CurrentSession;
            switch (Role)
            {
                case AccountType.All:
                    return true;

                case AccountType.Admin:
                    return currentSession.IsAdmin;

                case AccountType.CSD_IT:
                    return currentSession.IsAdmin && currentSession.LoginAccountType == AccountType.CSD_IT ? true : false;

                case AccountType.Licensee:
                    return currentSession.IsAdmin && currentSession.LoginAccountType == AccountType.Licensee ? true : false;

                case AccountType.Agent:
                    return !currentSession.IsAdmin ? true : false;

                case AccountType.AgentLevel0:
                    return !currentSession.IsAdmin && currentSession.AgentInfo.IsLevel0 ? true : false;

                case AccountType.AgentLevel1:
                    return !currentSession.IsAdmin && currentSession.AgentInfo.IsLevel1 ? true : false;

                case AccountType.AgentLeveln:
                    return !currentSession.IsAdmin && currentSession.AgentInfo.IsLastLevel ? true : false;

                default:
                    throw new Exception("Please implement here");
            }
        }
    }
}