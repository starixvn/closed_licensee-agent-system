// --------------------------------------------------------------------------
// <copyright file="IAuthorizeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAuthorizeService
    {
        void CheckDownlineBelong(CheckDownlineBelongCriteria criteria);

        bool IsDownlineBelong(int custId, int targetCustId, DownlineRole downlineRole);

        bool IsDownlineBelong(int targetCustId, DownlineRole downlineRole);

        bool ValidateDirectDownline(CheckDownlineBelongCriteria criteria);
    }
}