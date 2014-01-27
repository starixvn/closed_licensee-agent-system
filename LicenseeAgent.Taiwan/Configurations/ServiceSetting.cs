// ----------------------------------------------------------------------
// <copyright file="ServiceSetting.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ServiceSetting
    {
        public const string UrlAction = "{0}/{1}";

        public const string Method = "POST";

        public const string ContentType = "application/x-www-form-urlencoded";

        public const string ParamFormat = "{0}={1}&";

        public const string HttpError = "Server error (HTTP {0}: {1}).";

        public const int HttpErrorCode = 404;
    }
}
