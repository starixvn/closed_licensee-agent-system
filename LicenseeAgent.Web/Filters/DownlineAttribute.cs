// -----------------------------------------------------------------------
// <copyright file="DownlineAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Enumerations;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DownLineAttribute : BaseActionFilterAttribute
    {
        public DownLineAttribute()
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
            var agentId = GetDownLineParameter(filterContext, AgentIdParameterNames);
            var memberId = GetDownLineParameter(filterContext, MemberIdParameterNames);

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

            if (!AuthorizeService.IsDownline(criteria))
            {
                AccessDenied(filterContext);
            }
        }

        /// <summary>
        /// Try to get parameter value in action parameters and input stream.
        /// </summary>
        /// <param name="filterContext">Action executing context.</param>
        /// <param name="parameterNames">Parameter name array.</param>
        /// <returns>Long value.</returns>
        protected static long GetDownLineParameter(ActionExecutingContext filterContext, string[] parameterNames)
        {
            var value = GetDownLineParameter(filterContext.ActionParameters, parameterNames);
            if (value > 0)
            {
                return value;
            }

            return GetDownLineParameter(filterContext.HttpContext.Request.InputStream, parameterNames);
        }

        /// <summary>
        /// Try to get parameter value in action parameters.
        /// </summary>
        /// <param name="actionParameters">Action parameters.</param>
        /// <param name="parameterNames">Parameter name array.</param>
        /// <returns>Long value.</returns>
        protected static long GetDownLineParameter(IDictionary<string, object> actionParameters, string[] parameterNames)
        {
            foreach (var name in parameterNames)
            {
                if (!actionParameters.ContainsKey(name) || actionParameters[name] == null)
                {
                    continue;
                }

                var value = actionParameters[name];
                if (value.GetType() == typeof(long) && (long)value > 0)
                {
                    return (long)value;
                }

                if (value.GetType() == typeof(int) && (int)value > 0)
                {
                    return (long)value;
                }
            }

            return -1;
        }

        /// <summary>
        /// If data is submitted with JSON, try to get down line parameter in input stream.
        /// </summary>
        /// <param name="stream">Request input stream.</param>
        /// <param name="parameterNames">Parameter name array.</param>
        /// <returns>Long value.</returns>
        protected static long GetDownLineParameter(Stream stream, string[] parameterNames)
        {
            var requestText = string.Empty;
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream))
            {
                requestText = reader.ReadToEnd();
                reader.Close();
            }

            if (string.IsNullOrWhiteSpace(requestText))
            {
                return -1;
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