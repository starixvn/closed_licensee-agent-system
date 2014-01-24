// -----------------------------------------------------------------------
// <copyright file="AppHandler.cs" company="Nexcel">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System.Net;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class AppHandler
    {
        /// <summary>
        /// Gets the HTTP error message.
        /// </summary>
        /// <param name="httpCode">The HTTP code.</param>
        /// <returns>Returns the message by HTTP code.</returns>
        public static string GetHttpErrorMessage(int httpCode)
        {
            var message = string.Empty;
            switch (httpCode)
            {
                case (int)HttpStatusCode.Forbidden:
                    message = Las.HttpError_403;
                    break;

                case (int)HttpStatusCode.NotFound:
                    message = Las.HttpError_404;
                    break;

                default:
                    message = Las.HttpError_Default;
                    break;
            }

            return message;
        }
    }
}