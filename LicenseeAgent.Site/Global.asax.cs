// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Site
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Binders;
    using LicenseeAgent.Web.Mvc;

    //// Note: For instructions on enabling IIS6 or IIS7 classic mode,
    //// visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// A global MVC application class.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        protected void Application_BeginRequest()
        {
            // or any number of other checks, up to you
            if (Request.IsLocal)
            {
                // MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            // stop as early as you can, even earlier with MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);
            if (Request.IsLocal)
            {
                // MiniProfiler.Stop();
            }
        }

        /// <summary>
        /// Application Start Event Handler.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            ModelBinders.Binders.DefaultBinder = new AppModelBinder();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            DbSettingProviderManager.Current.Start();
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            if (AppSettings.LocalTest)
            {
                return;
            }

            var exception = Server.GetLastError();
            if (exception is HttpException)
            {
                return;
            }

            var antiForgeryException = exception as HttpAntiForgeryException;
            var isAntiForgeryException = antiForgeryException != null;
            var isLogging = !isAntiForgeryException;
            if (isLogging)
            {
                var baseException = exception.GetBaseException();
                string message = baseException.Message +
                    "\r\nSource: " + baseException.Source +
                    "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
                    "\r\nQueryString: " + Request.QueryString.ToString() +
                    "\r\nTargetSite: " + baseException.TargetSite +
                    "\r\nStackTrace: " + baseException.StackTrace;
                LogService.Error(message);
            }

            if (Response.IsRequestBeingRedirected)
            {
                return;
            }

            HandleException(exception);
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private void HandleException(Exception exception)
        {
            var httpException = exception as HttpException;
            var antiForgeryException = exception as HttpAntiForgeryException;
            var isAntiForgeryException = antiForgeryException != null;
            int errorCode = httpException != null ? httpException.GetHttpCode() : (int)HttpStatusCode.InternalServerError;
            Response.Clear();
            if (Request.IsAjaxRequest())
            {
                var msgContent = isAntiForgeryException ? Las.UnauthorizedMsg : AppHandler.GetHttpErrorMessage(errorCode);
                var jsonResult = new Result
                {
                    MsgId = errorCode,
                    MsgContent = msgContent
                };

                Response.WriteJson(jsonResult);
            }
            else
            {
                if (isAntiForgeryException)
                {
                    Response.Redirect(AppURLs.URLs.Unauthorized);
                }
                else
                {
                    Response.Redirect(string.Format("{0}/{1}", AppURLs.URLs.Error, errorCode));
                }
            }

            Server.ClearError();
        }
    }
}