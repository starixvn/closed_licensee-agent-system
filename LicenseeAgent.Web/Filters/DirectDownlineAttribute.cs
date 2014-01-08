// -----------------------------------------------------------------------
// <copyright file="DirectDownlineAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System.Web.Mvc;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DirectDownlineAttribute : DownlineAttribute
    {
        /// <summary>
        /// Check down line is belong.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            var agentId = GetDownlineParameter(filterContext, AgentIdParameterNames);
            var memberId = GetDownlineParameter(filterContext, MemberIdParameterNames);

            // Validation
            var invalidSession = currentSession == null || currentSession.AgentInfo == null;
            var invalidParameters = (agentId == -1 && memberId == -1) || (agentId > 0 && memberId > 0);
            if (invalidSession || invalidParameters)
            {
                return;
            }

            // Authorize
            var targetCustId = agentId > 0 ? agentId : memberId;
            var downlineRole = agentId > 0 ? DownlineRole.Agent : DownlineRole.Member;
            var criteria = new CheckDownlineBelongCriteria
            {
                TargetCustId = (int)targetCustId,
                Type = (int)downlineRole
            };

            if (!AuthorizeService.IsDirectDownline(criteria))
            {
                AccessDenied(filterContext);
            }
        }
    }
}