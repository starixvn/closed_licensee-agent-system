// --------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Mvc
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// Extensions class.
    /// </summary>
    public static class Extensions
    {
        public static bool IsPopup(this HttpRequestBase request)
        {
            var nullKeys = request.QueryString.GetValues(null);
            return nullKeys != null && nullKeys.Contains("popup");
        }

        public static string GetActionName(this HttpContextBase httpContext)
        {
            return httpContext.Request.RequestContext.RouteData.GetRequiredString("action");
        }

        public static string GetControllerName(this HttpContextBase httpContext)
        {
            return httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
        }

        public static string GetAreaName(this HttpContextBase httpContext)
        {
            return httpContext.Request.RequestContext.RouteData.Values["area"] as string;
        }

        public static int GetInt32Parameter(this ActionExecutingContext context, string name, int defaultValue = -1)
        {
            var actionParameters = context.ActionParameters;
            if (actionParameters.ContainsKey(name))
            {
                return Converter.ToInt(actionParameters[name], defaultValue);
            }

            return defaultValue;
        }

        public static bool IsAjaxRequest(this ControllerContext context)
        {
            return IsAjaxRequestInternal(context.HttpContext.Request);
        }

        public static bool IsJsonRequest(this ControllerContext context)
        {
            return IsAjaxRequest(context) && context.HttpContext.Request.ContentType.IndexOf("text/html", StringComparison.OrdinalIgnoreCase) == -1;
        }

        public static bool IsAjaxRequest(this HttpContext context)
        {
            var httpContextWrapper = new HttpContextWrapper(context);
            return IsAjaxRequestInternal(httpContextWrapper.Request);
        }

        public static bool IsJsonRequest(this HttpContext context)
        {
            return IsAjaxRequest(context) && context.Request.ContentType.IndexOf("text/html", StringComparison.OrdinalIgnoreCase) == -1;
        }

        public static void WriteJson(this HttpResponse httpResponse, Result jsonResult)
        {
            httpResponse.AddHeader("content-type", "application/json");
            httpResponse.Write(Formatter.FormatJson(jsonResult));
        }

        public static void WriteJson(this HttpResponseBase httpResponse, Result jsonResult)
        {
            httpResponse.AddHeader("content-type", "application/json");
            httpResponse.Write(Formatter.FormatJson(jsonResult));
        }

        private static bool IsAjaxRequestInternal(HttpRequestBase httpRequest)
        {
            var ajaxRequest = httpRequest.IsAjaxRequest();
            var jsonRequest = httpRequest.AcceptTypes.Contains("application/json");

            return ajaxRequest || jsonRequest;
        }
    }
}