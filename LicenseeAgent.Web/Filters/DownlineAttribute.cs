// -----------------------------------------------------------------------
// <copyright file="DownlineAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Enumerations;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DownlineAttribute : BaseActionFilterAttribute
    {
        public DownlineAttribute()
        {
            AuthorizeService = DependencyResolver.Current.GetService<IAuthorizeService>();
            AgentIdParameterNames = new[] { "AgentId", "CustId" };
            MemberIdParameterNames = new[] { "MemberId", "TargetCustId" };
            TransIdParameterNames = new[] { "TransId" };
        }

        protected static string[] AgentIdParameterNames { get; private set; }

        protected static string[] MemberIdParameterNames { get; private set; }

        protected static string[] TransIdParameterNames { get; private set; }

        protected IAuthorizeService AuthorizeService { get; private set; }

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

            AuthorizeService.IsDownline(criteria);
            if (!criteria.IsBelongTo)
            {
                AccessDenied(filterContext);
            }
        }

        /// <summary>
        /// Try to get parameter value in action parameters.
        /// </summary>
        /// <param name="filterContext">Action executing context.</param>
        /// <param name="parameterNames">Parameter name array.</param>
        /// <returns>Long value.</returns>
        protected static long GetDownlineParameter(ActionExecutingContext filterContext, string[] parameterNames)
        {
            var httpRequest = filterContext.HttpContext.Request;

            var value = GetDownlineParameter(filterContext, parameterNames);
            if (value > 0)
            {
                return value;
            }

            return GetDownlineParameter(filterContext.HttpContext.Request.InputStream, parameterNames);
        }

        /// <summary>
        /// If data is submitted with JSON, try to get down line parameter in input stream.
        /// </summary>
        /// <param name="stream">Request input stream.</param>
        /// <param name="parameterNames">Parameter name array.</param>
        /// <returns>Long value.</returns>
        protected static long GetDownlineParameter(Stream stream, string[] parameterNames)
        {
            var requestText = string.Empty;
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream))
            {
                requestText = reader.ReadToEnd();
                reader.Close();
            }

            foreach (var name in parameterNames)
            {
                var pattern = string.Format("\"{0}\":(\\d+)", name);
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                var match = regex.Match(requestText);
                if (match.Groups.Count != 2)
                {
                    continue;
                }

                long value = 0;
                var parseResult = long.TryParse(match.Groups[1].Value, out value);
                if (parseResult && value > 0)
                {
                    return value;
                }
            }

            return -1;
        }
    }
}