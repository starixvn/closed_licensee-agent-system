// --------------------------------------------------------------------------
// <copyright file="SessionManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.5.1">Created</change>
// </history>

namespace LicenseeAgent.Web
{
    using System.Web;

    /// <summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/ff650316.aspx"/> 
    /// </summary>
    public static class SessionManager
    {
        private const string AppSessionKey = "LicenseeAgent.Web.CurrentSession";

        public static ICurrentSession CurrentSession
        {
            get
            {
                return (ICurrentSession)HttpContext.Current.Session[AppSessionKey];
            }

            set
            {
                HttpContext.Current.Session[AppSessionKey] = value;
            }
        }

        public static void ClearAll()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Cookies.Clear();
            }
        }
    }
}