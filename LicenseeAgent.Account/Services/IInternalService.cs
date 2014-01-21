// --------------------------------------------------------------------------
// <copyright file="IInternalService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Account.Services
{
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IInternalService
    {
        string Encode(InternalLoginInfo internalLoginInfo);

        InternalLoginInfo Decode(string authKey);
    }
}