// ----------------------------------------------------------------------
// <copyright file="LogBinder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Utils
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
        /// <param name="action">Service Action.</param>
        /// <returns>String Action information.</returns>
        public static string GetActionInformation(IAction action)
        {
            var logContent = new LogContentEntity
            {
                Url = AppSettings.TaiwanAPI,
                Action = action.GetActionName(),
                Parameters = (LogAction)action
            };

            using (var stringWriter = new StringWriter())
            {
                var types = new Type[] 
                { 
                    typeof(InsertNewMemberAction), 
                    typeof(ResetPasswordAction), 
                    typeof(UpdateMemberInfoAction), 
                    typeof(UpdateMemberStatusAction) 
                };

                var serializer = new XmlSerializer(logContent.GetType(), types);

                serializer.Serialize(stringWriter, logContent);
             
                return stringWriter.ToString();
            }
        }
    }
}
