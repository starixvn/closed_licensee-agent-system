// -----------------------------------------------------------------------
// <copyright file="DownLineTransactionAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Web.Criterias;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class DownLineTransactionAttribute : DownLineAttribute
    {
        /// <summary>
        /// Check down line is belong.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            var transId = GetDownLineParameter(filterContext, TransIdParameterNames);

            // Validation
            var invalidSession = currentSession == null || currentSession.AgentInfo == null;
            if (invalidSession || transId <= 0)
            {
                return;
            }

            // Authorize
            var criteria = new TicketDownLineCriteria { TransId = transId };

            if (!AuthorizeService.IsDownlineTicket(criteria))
            {
                AccessDenied(filterContext);
            }
        }
    }
}