// --------------------------------------------------------------------------
// <copyright file="DownLineService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using Fanex.Common;
    using Fanex.Data;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Criterias;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DownLineService : QueryService, IDownLineService
    {
        /// <summary>
        /// Check if the target user is child of current user.
        /// </summary>
        /// <param name="criteria">Down line criteria.</param>
        /// <returns>Return boolean value indicating the down line value.</returns>
        public bool IsDownLine(CheckDownlineBelongCriteria criteria)
        {
            if (criteria.CustId == criteria.TargetCustId)
            {
                return true;
            }

            Execute(criteria);
            return criteria.IsBelongTo;
        }

        /// <summary>
        /// Check if the target user is direct child of current user.
        /// </summary>
        /// <param name="criteria">Down line criteria.</param>
        /// <returns>Return boolean value indicating the direct down line value.</returns>
        public bool IsDirectDownLine(CheckDownlineBelongCriteria criteria)
        {
            if (criteria.CustId == criteria.TargetCustId)
            {
                return true;
            }

            Execute(criteria);
            return criteria.IsBelongTo && criteria.IsDirectDownLine;
        }

        public bool IsIndirectDownLine(CheckDownlineBelongCriteria criteria)
        {
            if (criteria.CustId == criteria.TargetCustId)
            {
                return true;
            }

            Execute(criteria);
            return criteria.IsBelongTo && !criteria.IsDirectDownLine;
        }

        /// <summary>
        /// Check if the ticket is belong to current user.
        /// </summary>
        /// <param name="criteria">Ticket down line criteria.</param>
        /// <returns>Return boolean value indicating the down line value.</returns>
        public bool IsDownLineTicket(TicketDownLineCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return Converter.ToBool(objectDb.ExecuteScalar(param));
            }
        }

        /// <summary>
        /// Check if the reference number is belong to current user.
        /// </summary>
        /// <param name="criteria">Reference number down line criteria.</param>
        /// <returns>Return boolean value indicating the down line value.</returns>
        public bool IsDownLineReferenceNumber(ReferenceNumberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return Converter.ToBool(objectDb.ExecuteScalar(param));
            }
        }
    }
}