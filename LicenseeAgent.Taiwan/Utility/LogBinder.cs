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
        /// <typeparam name="T">Type action.</typeparam>
        /// <param name="action">Service Action.</param>
        /// <returns>String Action information.</returns>
        public static string GetActionInformation<T>(IAction action) where T : class
        {
            var logContent = new LogContentEntity
            {
                Link = AppSettings.TaiwanAPI,
                Action = action.ActionName,
                Parameters = (LogAction)action
            };

            return SerializeObject<LogContentEntity, T>(logContent);
        }

        /// <summary>
        /// Get member content.
        /// </summary>
        /// <param name="members">List member information.</param>
        /// <returns>Content of member.</returns>
        public static string GetMembersContent(IEnumerable<LogInformationEntity> members)
        {
            return SerializeObject<StatusEntity, LogInformationEntity>(new StatusEntity { Customers = members.ToList() });
        }

        /// <summary>
        /// Serialize object.
        /// </summary>
        /// <typeparam name="T1">Type of object 1 need serialize.</typeparam>
        /// <typeparam name="T2">Type of object 2 need serialize.</typeparam>
        /// <param name="objectSerialization">Object need serialize.</param>
        /// <returns>String object after serialize.</returns>
        private static string SerializeObject<T1, T2>(T1 objectSerialization) 
                where T1 : class 
                where T2 : class
        {
            using (var stringWriter = new ExtentedStringWriter(System.Text.Encoding.UTF8))
            {
                var serializer = new XmlSerializer(objectSerialization.GetType(), new Type[] { typeof(T1), typeof(T2) });

                serializer.Serialize(stringWriter, objectSerialization);

                return stringWriter.ToString();
            }
        }
    }
}
