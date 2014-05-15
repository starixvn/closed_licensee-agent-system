// --------------------------------------------------------------------------
// <copyright file="TrustedAdminAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System.Web.Mvc;

    /// <summary>
    /// TrustedAdminAttribute class.
    /// </summary>
    public class TrustedAdminAttribute : BaseActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            if (!currentSession.IsShowMaintenance())
            {
                Unauthorized(filterContext);
            }
        }
    }
}