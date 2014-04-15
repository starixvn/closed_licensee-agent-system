// --------------------------------------------------------------------------
// <copyright file="BaseActionFilterAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Filters
{
    using System.IO;
    using System.Web.Mvc;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// BaseActionFilterAttribute class.
    /// </summary>
    public abstract class BaseActionFilterAttribute : ActionFilterAttribute
    {
        protected static void Unauthorized(ActionExecutingContext filterContext)
        {
            filterContext.ReturnUnauthorized();
        }

        protected static void AccessDenied(ActionExecutingContext filterContext)
        {
            filterContext.ReturnAccessDenied();
        }

        /// <summary>
        /// Read text from request input stream.
        /// Seek to begin of stream before reading.
        /// </summary>
        /// <param name="filterContext">Action executing context.</param>
        /// <returns>Return text in stream.</returns>
        protected static string ReadRequestInputStream(ControllerContext filterContext)
        {
            var stream = filterContext.HttpContext.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream))
            {
                var text = reader.ReadToEnd();
                reader.Close();
                return text;
            }
        }
    }
}