// --------------------------------------------------------------------------
// <copyright file="PageExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Mvc
{
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Security;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        /// Returns the unauthorized.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public static void ReturnUnauthorized(this ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Clear();
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            response.TrySkipIisCustomErrors = true;
            if (filterContext.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new Result(Las.UnauthorizedMsg),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectResult(AppURLs.URLs.Unauthorized);
            }
        }

        /// <summary>
        /// Returns the unauthorized.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        public static void ReturnUnauthorized(this ControllerContext controllerContext)
        {
            var response = controllerContext.HttpContext.Response;
            response.Clear();
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            response.TrySkipIisCustomErrors = true;
            if (controllerContext.IsAjaxRequest())
            {
                response.WriteJson(new Result(Las.UnauthorizedMsg));
                response.End();
            }
            else
            {
                if (controllerContext.HttpContext.Response.IsRequestBeingRedirected)
                {
                    return;
                }

                response.Redirect(AppURLs.URLs.Unauthorized);
            }
        }

        /// <summary>
        /// Returns the unauthorized.
        /// </summary>
        /// <param name="authorizationContext">The authorization context.</param>
        public static void ReturnUnauthorized(this AuthorizationContext authorizationContext)
        {
            var response = authorizationContext.HttpContext.Response;
            response.Clear();
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            response.TrySkipIisCustomErrors = true;
            if (authorizationContext.IsAjaxRequest())
            {
                response.WriteJson(new Result(Las.UnauthorizedMsg));
                response.End();
            }
            else
            {
                if (authorizationContext.HttpContext.Response.IsRequestBeingRedirected)
                {
                    return;
                }

                FormsAuthentication.SignOut();
                authorizationContext.HttpContext.Session.Abandon();
                authorizationContext.HttpContext.Response.Cookies.Clear();
                authorizationContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
            }
        }

        /// <summary>
        /// Returns the access denied.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public static void ReturnAccessDenied(this ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Clear();
            response.StatusCode = (int)HttpStatusCode.Forbidden;
            response.TrySkipIisCustomErrors = true;
            if (filterContext.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new Result(Las.AccessDeniedMsg),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectResult(AppURLs.URLs.AccessDenied);
            }
        }

        /// <summary>
        /// Returns the access denied.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        public static void ReturnAccessDenied(this ControllerContext controllerContext)
        {
            var response = controllerContext.HttpContext.Response;
            response.Clear();
            response.StatusCode = (int)HttpStatusCode.Forbidden;
            response.TrySkipIisCustomErrors = true;
            if (controllerContext.IsAjaxRequest())
            {
                response.WriteJson(new Result(Las.AccessDeniedMsg));
                response.End();
            }
            else
            {
                if (controllerContext.HttpContext.Response.IsRequestBeingRedirected)
                {
                    return;
                }

                response.Redirect(AppURLs.URLs.AccessDenied);
            }
        }
    }
}