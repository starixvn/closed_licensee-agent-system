// --------------------------------------------------------------------------
// <copyright file="AppAuthorizeAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public sealed class AppAuthorizeAttribute : AuthorizeAttribute
    {
        private IAuthorizeService _permissionServiceBase;

        public AppAuthorizeAttribute()
        {
            _permissionServiceBase = DependencyResolver.Current.GetService<IAuthorizeService>();
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

            base.OnAuthorization(filterContext);

            if (!filterContext.HttpContext.Response.IsRequestBeingRedirected && filterContext.Result == null)
            {
                AuthorizeSession(filterContext);
            }

            if (!filterContext.HttpContext.Response.IsRequestBeingRedirected && filterContext.Result == null)
            {
                AuthorizeRole(filterContext);
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

        private void AuthorizeSession(AuthorizationContext filterContext)
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

            _permissionServiceBase.CheckCurrencyPermission(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                filterContext.ReturnCurrencyPermissionDenied();
            }
        }

        /// <summary>
        /// Authorizes the permission. Only call this function if the httpContext.Request.IsAuthenticated
        /// and session is valid.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <exception cref="System.NotSupportedException">Please implement here.</exception>
        /// <exception cref="System.Exception">Please implement here.</exception>
        private void AuthorizeRole(AuthorizationContext filterContext)
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
                    isAuthorized = currentSession.IsAdmin && currentSession.LoginAccountType == AccountType.CSD_IT ? true : false;
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

            if (!isAuthorized)
            {
                filterContext.ReturnAccessDenied();
            }
        }
    }
}