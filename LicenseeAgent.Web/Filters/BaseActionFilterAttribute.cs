// --------------------------------------------------------------------------
// <copyright file="BaseActionFilterAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System.Web.Mvc;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseActionFilterAttribute : ActionFilterAttribute
    {
        protected void Unauthorized(ActionExecutingContext filterContext)
        {
            filterContext.ReturnUnauthorized();
        }

        protected void AccessDenied(ActionExecutingContext filterContext)
        {
            filterContext.ReturnAccessDenied();
        }
    }
}