// ----------------------------------------------------------------------
// <copyright file="ServiceExtension.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Enumerations;
    using LicenseeAgent.Taiwan.Filter;
    using LicenseeAgent.Taiwan.Results;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Check is updated multiple status member.
        /// </summary>
        /// <param name="custId">List member ID.</param>
        /// <returns>If is multiple return true, else return false.</returns>
        public static bool IsUpdateMultipleStatus(string custId)
        {
            if (!string.IsNullOrEmpty(custId) && custId.Split(',').Count() <= 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update log Information.
        /// </summary>
        /// <param name="logInformations">List log information.</param>
        /// <param name="memberStatusResult">Member status result.</param>
        public static void UpdateLogInformations(IEnumerable<LogInformationEntity> logInformations, MemberStatusResult memberStatusResult)
        {
            if (memberStatusResult.ErrCode == 0)
            {
                ServiceMapper.MapObjects<MemberStatusEntity, LogInformationEntity>(memberStatusResult.MemberStatuses, logInformations, new MemberFilter());
            }
            else
            {
                foreach (var logInformation in logInformations)
                {
                    logInformation.Status = (short)LogType.Fail;
                }
            }
        }

        /// <summary>
        /// Get log information.
        /// </summary>
        /// <param name="custIds">Customer information ID.</param>
        /// <param name="agentIds">Agent information ID.</param>
        /// <returns>List log information.</returns>
        public static IEnumerable<LogInformationEntity> GetLogInformations(string custIds, string agentIds)
        {
            var logInformations = new List<LogInformationEntity>();

            if (!string.IsNullOrEmpty(custIds))
            {
                var listCustId = custIds.Split(',');
                logInformations.AddRange(listCustId.Select(custId => new LogInformationEntity
                {
                    AgentId = Convert.ToInt32(agentIds),
                    MemberId = Convert.ToInt32(custId),
                    Status = (short)LogType.BeforeCall
                }));
            }
            else if (!string.IsNullOrEmpty(agentIds))
            {
                var listAgentId = agentIds.Split(',');
                logInformations.AddRange(listAgentId.Select(agentId => new LogInformationEntity
                {
                    AgentId = Convert.ToInt32(agentId),
                    Status = (short)LogType.BeforeCall
                }));
            }

            return logInformations;
        }

        /// <summary>
        /// Check is parameter request.
        /// </summary>
        /// <param name="attribute">Attribute object.</param>
        /// <returns>If is parameter service return true, else return false.</returns>
        public static bool IsParamRequest(object attribute)
        {
            return attribute == null || ((ServiceAttribute)attribute).IsParamAction;
        }

        /// <summary>
        /// Write log when call API error.
        /// </summary>
        /// <param name="action">Action when call API.</param>
        /// <param name="result">Response Result.</param>
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
                var serviceAttribute = property.GetCustomAttributes(typeof(ServiceAttribute), true).FirstOrDefault();
                if (!ServiceExtension.IsParamRequest(serviceAttribute))
                {
                    continue;
                }

                var xmlIgnoreAttribute = property.GetCustomAttributes(typeof(XmlIgnoreAttribute), true).FirstOrDefault();
                if (xmlIgnoreAttribute != null)
                {
                    continue;
                }

                var logName = serviceAttribute != null ? ((ServiceAttribute)serviceAttribute).Name : property.Name;
                var logValue = propertyValue != null ? HttpUtility.UrlEncode(propertyValue.ToString()) : string.Empty;
                errorMessage.AppendFormat("{0}: {1}\n", logName, logValue);
            }

            errorMessage.Append("Output:\n");
            errorMessage.AppendFormat("Error Code: {0}\n", result.ErrCode);
            errorMessage.AppendFormat("Error Message: {0}\n", result.ErrMsg);

            Logger.Error(errorMessage.ToString());
        }
    }
}