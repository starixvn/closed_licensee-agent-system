// --------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Site
{
    using System.Web.Mvc;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// MVC Global Filter Configuration.
    /// </summary>
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AppAuthorizeAttribute());
            filters.Add(new UnderMaintenanceAttribute());
        }
    }
}