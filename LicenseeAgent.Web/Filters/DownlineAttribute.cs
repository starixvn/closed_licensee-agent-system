// -----------------------------------------------------------------------
// <copyright file="DownlineAttribute.cs" company="Nexcel Solutions Vietnam">
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
    public class DownLineAttribute : BaseActionFilterAttribute
    {
        static DownLineAttribute()
        {
            AgentIdParameterNames = new[] { "AgentId", "CustId" };
            MemberIdParameterNames = new[] { "MemberId", "TargetCustId" };
            TransIdParameterNames = new[] { "TransId" };
        }

        public DownLineAttribute()
        {
            AuthorizeService = DependencyResolver.Current.GetService<IAuthorizeService>();
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
            var validSession = currentSession != null && currentSession.AgentInfo != null;
            var validParameters = (agentId > 0) ^ (memberId > 0);

            if (!validSession || !validParameters)
            {
                return;
            }

            // Authorize
            var targetCustId = agentId > 0 ? agentId : memberId;
            var downlineRole = agentId > 0 ? DownLineRole.Agent : DownLineRole.Member;
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
            var actionParameters = GetActionParameter(filterContext.ActionParameters, parameterNames);
            var jsonParameters = GetJsonParameter(filterContext.HttpContext.Request.InputStream, parameterNames);
            var allParameters = actionParameters.Union(jsonParameters).ToArray();

            if (allParameters.Length > 1)
            {
                var exceptionMessage = string.Format(Las.TooManyDownLineParameters, string.Join(",", parameterNames));
                throw new ArgumentException("There is too many parameters with name {0}", exceptionMessage);
            }

            return allParameters.Length == 1 ? allParameters[0] : -1;
        }

        /// <summary>
        /// Try to get parameter value in action parameters.
        /// </summary>
        /// <param name="actionParameters">Action parameters.</param>
        /// <param name="parameterNames">Parameter name array.</param>
        /// <returns>Long value.</returns>
        protected static IEnumerable<long> GetActionParameter(IDictionary<string, object> actionParameters, string[] parameterNames)
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
                    yield return (long)value;
                }

                if (value.GetType() == typeof(int) && (int)value > 0)
                {
                    yield return (long)(int)value;
                }
            }
        }

        /// <summary>
        /// If data is submitted with JSON, try to get down line parameter in input stream.
        /// </summary>
        /// <param name="stream">Request input stream.</param>
        /// <param name="parameterNames">Parameter name array.</param>
        /// <returns>Long value.</returns>
        protected static IEnumerable<long> GetJsonParameter(Stream stream, string[] parameterNames)
        {
            if (stream.Length == 0)
            {
                yield break;
            }

            var requestText = ReadString(stream);
            foreach (var name in parameterNames)
            {
                var pattern = string.Format("\"{0}\":(\\d+)", name);
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                var values = regex.Matches(requestText)
                                   .Cast<Match>()
                                   .Where(m => m.Groups.Count == 2)
                                   .Select(m => m.Groups[1].Value)
                                   .Select(m => long.Parse(m));
                foreach (var value in values)
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Read text from stream.
        /// Seek to begin of stream before reading.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <returns>Return text in stream.</returns>
        private static string ReadString(Stream stream)
        {
            var text = string.Empty;
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
                reader.Close();
            }

            return text;
        }
    }
}