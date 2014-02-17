// ----------------------------------------------------------------------
// <copyright file="LogBinder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
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
                Link = AppSettings.TaiwanAPI,
                Action = action.ActionName,
                Parameters = (LogAction)action
            };

            return SerializeObject<LogContentEntity>(logContent, action.GetType());
        }

        /// <summary>
        /// Get member content.
        /// </summary>
        /// <param name="members">List member information.</param>
        /// <returns>Content of member.</returns>
        public static string GetMembersContent(IEnumerable<LogInformationEntity> members)
        {
            return SerializeObject<StatusEntity>(
                new StatusEntity
                {
                    Customers = new Collection<LogInformationEntity>(members.ToList())
                }, 
                typeof(LogInformationEntity));
        }

        /// <summary>
        /// Serialize object.
        /// </summary>
        /// <typeparam name="T">Type of object need serialize.</typeparam>        
        /// <param name="objectSerialization">Object need serialize.</param>
        /// <param name="typeEntity">Type need serialize.</param>
        /// <returns>String object after serialize.</returns>
        private static string SerializeObject<T>(T objectSerialization, Type typeEntity)
        {
            using (var stringWriter = new ExtentedStringWriter(System.Text.Encoding.UTF8))
            {
                var serializer = new XmlSerializer(objectSerialization.GetType(), new Type[] { typeEntity });

                serializer.Serialize(stringWriter, objectSerialization);

                return stringWriter.ToString();
            }
        }
    }
}
