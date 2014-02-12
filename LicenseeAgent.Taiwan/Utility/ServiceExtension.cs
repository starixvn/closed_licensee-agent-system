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

        public static IEnumerable<LogInformationEntity> GetLogInformations(string custIds, string agentIds)
        {
            var logInformations = new List<LogInformationEntity>();

            if (!string.IsNullOrEmpty(custIds))
            {
                var listCustId = custIds.Split(',');
                foreach (var custId in listCustId)
                {
                    logInformations.Add(new LogInformationEntity
                    {
                        AgentId = Convert.ToInt32(agentIds),
                        MemberId = Convert.ToInt32(custId),
                        Status = (short)LogType.BeforeCall
                    });
                }
            }
            else
            {
                var listAgentId = agentIds.Split(',');
                foreach (var agentId in listAgentId)
                {
                    logInformations.Add(new LogInformationEntity
                    {
                        AgentId = Convert.ToInt32(agentId),
                        Status = (short)LogType.BeforeCall
                    });
                }
            }

            return logInformations;
        }
    }
}
