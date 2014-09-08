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
        private static bool IsAllowLogging
        {
            get
            {
                return !AppSettings.IsLocalRequest;
            }
        }

        public static void HandleException(Exception exception)
        {
            HandleException("Exception Log", exception);
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void HandleException(string message, Exception exception)
        {
            if (AppSettings.IsLocalRequest)
            {
                throw exception;
            }

            if (IsAllowLogging)
            {
                LogService.Error(message, exception);
            }
        }

        public static void Error(string message)
        {
            if (IsAllowLogging)
            {
                LogService.Error(message);
            }
        }

        public static void Debug(string message)
        {
            if (IsAllowLogging)
            {
                LogService.Debug(message);
            }
        }
    }
}