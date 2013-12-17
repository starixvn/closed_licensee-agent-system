// --------------------------------------------------------------------------
// <copyright file="BaseActionFilterAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Web.Filters
{
    using System.Net;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Return page access denied once user dose not permission.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected void DoUnAuthorized(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            if (filterContext.IsAjaxRequest())
            {
                response.Clear();
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                var jsonResult = new Result(Las.UnauthorizedMsg);
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.End();
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = AppURLs.URLs.AccessDenied
                };
            }
        }
    }
}