// -----------------------------------------------------------------------
// <copyright file="DirectDownLineAgentAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class DirectDownLineAgentAttribute : DownLineAttribute
    {
        /// <summary>
        /// Check down line is belong.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            var agentId = GetDownLineParameter(filterContext, AgentIdParameterNames);

            // Validation
            var invalidSession = currentSession == null || currentSession.AgentInfo == null;
            if (invalidSession || agentId <= 0)
            {
                return;
            }

            // Authorize
            var criteria = new CheckDownlineBelongCriteria
            {
                TargetCustId = (int)agentId,
                Type = (int)DownLineRole.Agent
            };

            if (!AuthorizeService.IsDirectDownline(criteria))
            {
                if (!criteria.IsBelongTo)
                {
                    AccessDenied(filterContext);
                }
                else if (!criteria.IsDirectDownLine)
                {
                    if (IsAllowAdmin && currentSession.IsAdmin)
                    {
                        return;
                    }

                    AccessDenied(filterContext);
                }
            }
        }
    }
}