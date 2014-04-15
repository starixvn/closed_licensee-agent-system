// --------------------------------------------------------------------------
// <copyright file="SitePath.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Core
{
    using System.IO;
    using System.Web;

    /// <summary>
    /// Site path configuration.
    /// </summary>
    public static class SitePath
    {
        public static readonly string RootPath = HttpContext.Current.Server.MapPath("~");

        public static readonly string GlobalImagePath = Path.Combine(RootPath, "Images");
    }
}