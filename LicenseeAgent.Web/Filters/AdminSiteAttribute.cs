// --------------------------------------------------------------------------
// <copyright file="AdminSiteAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AdminSiteAttribute : BaseActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!AppSettings.IsAdminSite)
            {
                Unauthorized(filterContext);
            }
        }
    }
}