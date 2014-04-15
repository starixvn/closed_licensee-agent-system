// --------------------------------------------------------------------------
// <copyright file="UnderMaintenanceAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// UnderMaintenanceAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class UnderMaintenanceAttribute : ActionFilterAttribute, IDisposable
    {
        private IUnderMaintenanceService _maintenanceService;

        public UnderMaintenanceAttribute()
        {
            _maintenanceService = DependencyResolver.Current.GetService<IUnderMaintenanceService>();
        }

        public bool Ignore { get; set; }

        /// <summary>
        /// Override base action executing function to check maintenance before executing request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Ignore)
            {
                return;
            }

            var maintenanceInfo = _maintenanceService.GetUnderMaintenanceInfo();
            if (maintenanceInfo.Status)
            {
                filterContext.ReturnUnderMaintenance();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c>
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            _maintenanceService.Dispose();
        }
    }
}