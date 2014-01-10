// -----------------------------------------------------------------------
// <copyright file="DownLineMemberAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Enumerations;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DownLineMemberAttribute : DownLineAttribute
    {
        private static string[] _memberIdParameterNames = new[] { "MemberId", "CustId", "TargetCustId" };

        /// <summary>
        /// Check down line is belong.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            var memberId = GetDownLineParameter(filterContext, _memberIdParameterNames);

            // Validation
            var validSession = currentSession != null && currentSession.AgentInfo != null;
            if (!validSession || memberId <= 0)
            {
                return;
            }

            // Authorize
            var criteria = new CheckDownlineBelongCriteria
            {
                TargetCustId = (int)memberId,
                Type = (int)DownLineRole.Member
            };

            if (!AuthorizeService.IsDownline(criteria))
            {
                AccessDenied(filterContext);
            }
        }
    }
}