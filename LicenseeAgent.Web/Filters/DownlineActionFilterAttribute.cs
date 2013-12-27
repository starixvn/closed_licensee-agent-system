// --------------------------------------------------------------------------
// <copyright file="DownlineActionFilterAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DownlineActionFilterAttribute : BaseActionFilterAttribute
    {
        private IAuthorizeService _authorizeService;

        public DownlineActionFilterAttribute()
        {
            _authorizeService = DependencyResolver.Current.GetService<IAuthorizeService>();
        }

        /// <summary>
        /// Check down line is belong.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            var agentId = filterContext.GetInt32Parameter("agentId", -1);
            var memberId = filterContext.GetInt32Parameter("memberId", -1);

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
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = targetCustId,
                Type = (int)downlineRole
            };
            _authorizeService.CheckDownlineBelong(criteria);
            if (!criteria.IsBelongTo)
            {
                AccessDenied(filterContext);
            }
            else
            {
                var isValidateDirectDownline = filterContext.ActionDescriptor.IsDefined(typeof(DirectDownlineAttribute), true)
                    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(DirectDownlineAttribute), true);
                if (!_authorizeService.ValidateDirectDownline(criteria))
                {
                    AccessDenied(filterContext);
                }
            }
        }
    }
}