namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// LegendTypeAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class LegendTypeAttribute : BaseActionFilterAttribute
    {
        public LegendTypeAttribute(LegendType legendType)
        {
            LegendType = legendType;
        }

        public LegendType LegendType { get; private set; }

        /// <summary>
        /// Authorizes the specified legend type.
        /// </summary>
        /// <param name="legendType">Type of the legend.</param>
        /// <returns>Returns boolean.</returns>
        public static bool Authorize(LegendType legendType)
        {
            var currentSession = SessionManager.CurrentSession;
            if (currentSession.AgentInfo == null)
            {
                return false;
            }

            var isAuthorized = false;
            switch (legendType)
            {
                case LegendType.Credit:
                    isAuthorized = currentSession.AgentInfo.LegendType == LegendType.Credit;
                    break;

                case LegendType.Deposit:
                    isAuthorized = currentSession.AgentInfo.LegendType == LegendType.Deposit;
                    break;
            }

            return isAuthorized;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Authorize(LegendType))
            {
                NotFound(filterContext);
            }
        }
    }
}