// --------------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.7">Created</change>
// </history>

namespace LicenseeAgent.Core.Services
{
    using System;
    using Fanex.Logging;
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>
        /// Handle exception by using FANEX Logging Lib to write log.
        /// </summary>
        /// <param name="exception">Exception object.</param>
        protected void HandleException(Exception exception)
        {
            HandleException(exception.Message, exception);
        }

        protected void HandleException(string message, Exception exception)
        {
            if (AppSettings.LocalTest)
            {
                throw exception;
            }

            LogService.Error(message, exception);
        }
    }
}