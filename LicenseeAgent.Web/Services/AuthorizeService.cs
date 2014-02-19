// --------------------------------------------------------------------------
// <copyright file="AuthorizeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using Fanex.Data;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Criterias;
    using LicenseeAgent.Core.Entities;
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
            if (!AppSettings.IsCheckingBelonging)
            {
                return true;
            }

            if (criteria.CustId == 0)
            {
                var agentId = SessionManager.CurrentSession.AgentInfo.AgentId;
                criteria.CustId = agentId;
            }

            if (criteria.CustId == criteria.TargetCustId)
            {
                return false;
            }

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
            if (!AppSettings.IsCheckingBelonging)
            {
                return true;
            }

            if (criteria.CustId == 0)
            {
                var agentId = SessionManager.CurrentSession.AgentInfo.AgentId;
                criteria.CustId = agentId;
            }

            if (criteria.CustId == criteria.TargetCustId)
            {
                return false;
            }
            
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
            if (!AppSettings.IsCheckingBelonging)
            {
                return true;
            }

            if (criteria.AgentId == 0)
            {
                var agentId = SessionManager.CurrentSession.AgentInfo.AgentId;
                criteria.AgentId = agentId;
            }

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return (bool)objectDb.ExecuteScalar(param);
            }
        }

        /// <summary>
        /// Check if the ticket is belong to current user.
        /// </summary>
        /// <param name="criteria">Ticket down line criteria.</param>
        /// <returns>Return boolean value indicating the down line value.</returns>
        public bool IsDownlineTicketDetails(TicketDetailsDownLineCriteria criteria)
        {
            if (!AppSettings.IsCheckingBelonging)
            {
                return true;
            }

            if (criteria.AgentId == 0)
            {
                var agentId = SessionManager.CurrentSession.AgentInfo.AgentId;
                criteria.AgentId = agentId;
            }

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
            if (!AppSettings.IsCheckingBelonging)
            {
                return true;
            }

            if (criteria.AgentId == 0)
            {
                var agentId = SessionManager.CurrentSession.AgentInfo.AgentId;
                criteria.AgentId = agentId;
            }

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return (bool)objectDb.ExecuteScalar(param);
            }
        }

        public bool IsValidCurrency(InternalLoginAsUserCriteria internalLoginAsUserCriteria)
        {
            if (!AppSettings.IsCheckingCurrencyPermission)
            {
                return true;
            }

            Execute(internalLoginAsUserCriteria);
            return internalLoginAsUserCriteria.ErrCode == Result.SuccessValue ? true : false;
        }
    }
}