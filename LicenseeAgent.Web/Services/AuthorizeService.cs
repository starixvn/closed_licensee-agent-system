// --------------------------------------------------------------------------
// <copyright file="AuthorizeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using Fanex.Data;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthorizeService : QueryService, IAuthorizeService
    {
        /// <summary>
        /// Check if the target user is direct child of current user.
        /// </summary>
        /// <param name="criteria">Down line criteria.</param>
        /// <returns>Return boolean value indicating the direct down line value.</returns>
        public bool IsDirectDownline(CheckDownlineBelongCriteria criteria)
        {
            var agentId = SessionManager.CurrentSession.AgentInfo.AgentId;
            if (agentId == criteria.TargetCustId)
            {
                return true;
            }

            criteria.CustId = agentId;
            Execute(criteria);

            return criteria.Type == (int)DownLineRole.Agent
                ? criteria.IsDirectDownLine
                : criteria.IsBelongTo && SessionManager.CurrentSession.AgentInfo.IsLastLevel;
        }

        /// <summary>
        /// Check if the target user is child of current user.
        /// </summary>
        /// <param name="criteria">Down line criteria.</param>
        /// <returns>Return boolean value indicating the down line value.</returns>
        public bool IsDownline(CheckDownlineBelongCriteria criteria)
        {
            var agentId = SessionManager.CurrentSession.AgentInfo.AgentId;
            if (agentId == criteria.TargetCustId)
            {
                return true;
            }

            criteria.CustId = agentId;
            Execute(criteria);

            return criteria.IsBelongTo;
        }

        /// <summary>
        /// Check if the ticket is belong to current user.
        /// </summary>
        /// <param name="criteria">Ticket down line criteria.</param>
        /// <returns>Return boolean value indicating the down line value.</returns>
        public bool IsDownlineTicket(TicketDownLineCriteria criteria)
        {
            criteria.AgentId = SessionManager.CurrentSession.AgentInfo.AgentId;
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return (bool)objectDb.ExecuteScalar(param);
            }
        }

        /// <summary>
        /// Check if the reference number is belong to current user.
        /// </summary>
        /// <param name="criteria">Reference number down line criteria.</param>
        /// <returns>Return boolean value indicating the down line value.</returns>
        public bool IsDownlineReferenceNumber(ReferenceNumberCriteria criteria)
        {
            criteria.AgentId = SessionManager.CurrentSession.AgentInfo.AgentId;
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return (bool)objectDb.ExecuteScalar(param);
            }
        }
    }
}