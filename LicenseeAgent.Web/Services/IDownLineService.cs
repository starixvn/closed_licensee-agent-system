// --------------------------------------------------------------------------
// <copyright file="IDownLineService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Web.Criterias;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDownLineService
    {
        bool IsDirectDownline(CheckDownlineBelongCriteria criteria);

        bool IsDownline(CheckDownlineBelongCriteria criteria);

        bool IsDownlineTicket(TicketDownLineCriteria criteria);

        bool IsDownlineTicketDetails(TicketDetailsDownLineCriteria criteria);

        bool IsDownlineReferenceNumber(ReferenceNumberCriteria criteria);
    }
}