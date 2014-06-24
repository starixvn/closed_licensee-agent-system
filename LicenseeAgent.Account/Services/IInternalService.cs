// --------------------------------------------------------------------------
// <copyright file="IInternalService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Account.Services
{
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.API.LRF.Entities;
    using LicenseeAgent.Core.Entities;

    public interface IInternalService
    {
        string Encode(InternalLoginInfo internalLoginInfo);

        bool IsExpired(LoginInfo loginInfo);

        InternalLoginInfo Decode(string authKey);

        AgentInfo AgentInternalLogin(AgentInternalLoginCriteria criteria);
    }
}