// -----------------------------------------------------------------------
// <copyright file="DirectDownLineMemberAttribute.cs" company="Nexcel Solutions Vietnam">
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
    public sealed class DirectDownLineMemberAttribute : DownLineAttribute
    {
        public DirectDownLineMemberAttribute()
        {
            IsAllowAdmin = true;
        }

        public bool IsAllowAdmin { get; set; }

        /// <summary>
        /// Check down line is belong.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            var memberId = GetDownLineParameter(filterContext, MemberIdParameterNames);

            // Validation
            var invalidSession = currentSession == null || currentSession.AgentInfo == null;
            if (invalidSession || memberId <= 0)
            {
                return;
            }

            // Authorize
            var criteria = new CheckDownlineBelongCriteria
            {
                TargetCustId = (int)memberId,
                Type = (int)DownLineRole.Member
            };

            if (!AuthorizeService.IsDirectDownline(criteria))
            {
                if (!criteria.IsBelongTo)
                {
                    AccessDenied(filterContext);
                }
                else
                {
                    if (!criteria.IsDirectDownLine)
                    {
                        if (IsAllowAdmin && currentSession.IsAdmin)
                            return;
                        AccessDenied(filterContext);
                    }
                }
            }
        }
    }
}