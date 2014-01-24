// ----------------------------------------------------------------------
// <copyright file="LogBinder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Utility
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LogBinder
    {
        /// <summary>
        /// Get action information.
        /// </summary>
        /// <typeparam name="T">Type action.</typeparam>
        /// <param name="action">Service Action.</param>
        /// <returns>String Action information.</returns>
        public static string GetActionInformation<T>(IAction action) where T : class
        {
            var logContent = new LogContentEntity
            {
                Link = AppSettings.TaiwanAPI,
                Action = action.GetActionName(),
                Parameters = (LogAction)action
            };

            using (var stringWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(logContent.GetType(), new Type[] { typeof(T) });

                serializer.Serialize(stringWriter, logContent);
             
                return stringWriter.ToString();
            }
        }
    }
}
