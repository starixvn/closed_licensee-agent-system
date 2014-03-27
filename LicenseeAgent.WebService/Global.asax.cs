// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.WebService
{
    using System.Web;
    using Fanex.Data.Configuration;

    //// Note: For instructions on enabling IIS6 or IIS7 classic mode,
    //// visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// A global MVC application class.
    /// </summary>
    public class LicenseeAgentApplication : HttpApplication
    {
        /// <summary>
        /// Application Start Event Handler.
        /// </summary>
        protected static void Application_Start()
        {
            DbSettingProviderManager.Current.Start();
        }
    }
}