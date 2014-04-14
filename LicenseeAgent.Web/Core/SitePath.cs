// --------------------------------------------------------------------------
// <copyright file="SitePath.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.29">Created</change>
// </history>

namespace LicenseeAgent.Web.Core
{
    using System.IO;
    using System.Web;

    public static class SitePath
    {
        public static readonly string RootPath = HttpContext.Current.Server.MapPath("~");

        public static readonly string GlobalImagePath = Path.Combine(RootPath, "Images");
    }
}