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

            var currentSession = SessionManager.CurrentSession;
            if (currentSession != null && currentSession.IsValid)
            {
                var exception = Server.GetLastError().GetBaseException();
                string message = exception.Message +
                    "\r\nSource: " + exception.Source +
                    "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
                    "\r\nQueryString: " + Request.QueryString.ToString() +
                    "\r\nTargetSite: " + exception.TargetSite +
                    "\r\nStackTrace: " + exception.StackTrace;
                LogService.Error(message);
            }

            var serverError = Server.GetLastError() as HttpException;
            int errorCode = serverError != null ? serverError.GetHttpCode() : (int)HttpStatusCode.InternalServerError;
            if (!Response.IsRequestBeingRedirected)
            {
                Response.Clear();
                var httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
                if (Request.IsAjaxRequest())
                {
                    var jsonResult = new Result
                    {
                        MsgId = errorCode,
                        MsgContent = AppHandler.GetHttpErrorMessage(errorCode)
                    };

                    Response.WriteJson(jsonResult);
                }
                else
                {
                    Response.Redirect(string.Format("{0}/{1}", AppURLs.URLs.Error, errorCode));
                }

                Server.ClearError();
            }
        }
    }
}