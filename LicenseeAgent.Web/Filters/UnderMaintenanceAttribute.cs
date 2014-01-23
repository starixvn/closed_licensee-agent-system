// --------------------------------------------------------------------------
// <copyright file="UnderMaintenanceAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
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
                var isAjaxRequest = filterContext.IsAjaxRequest();
                filterContext.Result = isAjaxRequest
                    ? (ActionResult)(new JsonResult { Data = ((ViewResult)filterContext.Result).ViewData.Model })
                    : (ActionResult)(new RedirectResult(AppURLs.URLs.UnderMaintenance, false));
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