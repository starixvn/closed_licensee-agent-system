// --------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System;
    using Fanex.Logging;
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Logger
    {
        public static void HandleException(Exception exception)
        {
            HandleException(exception.Message, exception);
        }

        public static void HandleException(string message, Exception exception)
        {
            if (AppSettings.LocalTest)
            {
                throw exception;
            }

            LogService.Error(message, exception);
        }
    }
}