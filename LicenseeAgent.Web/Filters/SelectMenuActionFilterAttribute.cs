// --------------------------------------------------------------------------
// <copyright file="SelectMenuActionFilterAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Web.Filters
{
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SelectMenuActionFilterAttribute : BaseActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentSession = SessionManager.CurrentSession;
            if (currentSession != null)
            {
                if (currentSession.AgentInfo != null && !currentSession.AgentInfo.IsLevel0)
                {
                    DoUnAuthorized(filterContext);
                }
            }
        }
    }
}