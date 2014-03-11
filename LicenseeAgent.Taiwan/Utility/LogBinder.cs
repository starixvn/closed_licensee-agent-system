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
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Results;

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
        /// Write log when call API error.
        /// </summary>
        /// <param name="action">Action when call API.</param>
        /// <param name="result">Response result.</param>
        public static void WriteError(IAction action, ResponseResult result)
        {
            if (result.ErrCode == 0)
            {
                return;
            }

            var errorMessage = new StringBuilder();
            errorMessage.AppendFormat("API function: {0}\n", action.ActionName);
            errorMessage.Append("Input:\n");

            var properties = action.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(action, null);
                var attribute = property.GetCustomAttributes(typeof(ServiceAttribute), true).FirstOrDefault();

                if (!ServiceExtension.IsParamRequest(attribute))
                {
                    continue;
                }

                errorMessage.AppendFormat(
                    "{0}: {1}\n",
                    attribute != null ? ((ServiceAttribute)attribute).Name : property.Name,
                    propertyValue != null ? HttpUtility.UrlEncode(propertyValue.ToString()) : string.Empty);
            }

            errorMessage.Append("Output:\n");
            errorMessage.AppendFormat("Error Code: {0}\n", result.ErrCode);
            errorMessage.AppendFormat("Error Message: {0}\n", result.ErrMsg);
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
            using (var stringWriter = new ExtentedStringWriter(Encoding.UTF8))
            {
                var serializer = new XmlSerializer(objectSerialization.GetType(), new Type[] { typeEntity });

                serializer.Serialize(stringWriter, objectSerialization);

                return stringWriter.ToString();
            }
        }
    }
}
