// --------------------------------------------------------------------------
// <copyright file="IAuthorizeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Web.Criterias;

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