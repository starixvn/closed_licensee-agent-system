// --------------------------------------------------------------------------
// <copyright file="AuthorizeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Criterias;

    /// <summary>
    /// AuthorizeService class.
    /// </summary>
    public class AuthorizeService : QueryService, IAuthorizeService
    {
        public void CheckCurrencyPermission(CheckCurrencyPermissionCriteria criteria)
        {
            Execute(criteria);
        }
    }
}