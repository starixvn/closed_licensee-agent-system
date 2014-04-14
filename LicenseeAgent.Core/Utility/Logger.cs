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

    public static class Logger
    {
        public static void HandleException(Exception exception)
        {
            HandleException("Exception Log", exception);
        }

        public static void HandleException(string message, Exception exception)
        {
            if (AppSettings.LocalTest)
            {
                throw exception;
            }

            LogService.Error(message, exception);
        }

        public static void Error(string message)
        {
            if (AppSettings.LocalTest)
            {
                return;
            }

            LogService.Error(message);
        }
    }
}