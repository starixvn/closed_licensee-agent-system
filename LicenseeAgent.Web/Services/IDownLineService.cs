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
        bool IsDownLine(CheckDownlineBelongCriteria criteria);

        bool IsDirectDownLine(CheckDownlineBelongCriteria criteria);

        bool IsIndirectDownLine(CheckDownlineBelongCriteria criteria);

        bool IsDownLineTicket(TicketDownLineCriteria criteria);

        bool IsDownLineReferenceNumber(ReferenceNumberCriteria criteria);
    }
}