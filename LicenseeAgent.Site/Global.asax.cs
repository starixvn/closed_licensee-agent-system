// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Site
{
    using System;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.Data.Configuration;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;
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
        private const int ServerErrorCode = 500;

        protected void Application_BeginRequest()
        {
            // or any number of other checks, up to you
            if (AppSettings.LocalTest)
            {
                // MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            // stop as early as you can, even earlier with MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);
            if (AppSettings.LocalTest)
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
            HandleRequest(exception);
            LogException(exception);
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private void HandleRequest(Exception exception)
        {
            if (Response.IsRequestBeingRedirected)
            {
                return;
            }

            var httpException = exception as HttpException;
            var antiForgeryException = exception as HttpAntiForgeryException;
            var isAntiForgeryException = antiForgeryException != null;
            int errorCode = httpException != null ? httpException.GetHttpCode() : (int)HttpStatusCode.InternalServerError;
            Response.Clear();
            if (HttpContext.Current.IsAjaxRequest())
            {
                var msgContent = isAntiForgeryException ? Las.UnauthorizedMsg : AppHandler.GetHttpErrorMessage(errorCode);
                var jsonResult = new Result
                {
                    ErrCode = errorCode,
                    ErrMsg = msgContent
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

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private void LogException(Exception exception)
        {
            var baseException = exception.GetBaseException();
            var stringBuilder = new StringBuilder(baseException.Message);
            stringBuilder.AppendFormat("\nSource: {0}", baseException.Source);
            stringBuilder.AppendFormat("\nDomain: {0}", HttpContext.Current.Request.Headers["Host"]);
            stringBuilder.AppendFormat("\nQueryString: {0}", Request.QueryString.ToString());
            stringBuilder.AppendFormat("\nTargetSite: {0}", baseException.TargetSite);
            stringBuilder.AppendFormat("\nStackTrace: {0}", baseException.StackTrace);
            if (SessionManager.CurrentSession != null)
            {
                stringBuilder.AppendFormat("\nLoginUserId: {0}", SessionManager.CurrentSession.LoginUserId);
                if (SessionManager.CurrentSession.AgentInfo != null)
                {
                    var agentInfo = SessionManager.CurrentSession.AgentInfo;
                    stringBuilder.AppendFormat("\nAgentId: {0}", agentInfo.AgentId);
                    stringBuilder.AppendFormat("\nLicenseeId: {0}", agentInfo.LicenseeId);
                    stringBuilder.AppendFormat("\nCurrencyId: {0}", agentInfo.CurrencyId);
                }
            }

            Logger.Error(stringBuilder.ToString());
        }

        private bool IsInternalServerError(Exception exception)
        {
            var httpException = exception as HttpException;
            if (httpException == null)
            {
                return false;
            }

            return httpException.GetHttpCode() >= ServerErrorCode;
        }
    }
}