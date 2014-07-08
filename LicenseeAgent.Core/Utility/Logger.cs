namespace LicenseeAgent.Core.Utility
{
    using System;
    using Fanex.Logging;
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// Logger class.
    /// </summary>
    public static class Logger
    {
        public static void HandleException(Exception exception)
        {
            HandleException("Exception Log", exception);
        }

        public static void HandleException(string message, Exception exception)
        {
            if (AppSettings.IsLocalRequest)
            {
                throw exception;
            }

            LogService.Error(message, exception);
        }

        public static void Error(string message)
        {
            if (AppSettings.IsLocalRequest)
            {
                return;
            }

            LogService.Error(message);
        }
    }
}