// --------------------------------------------------------------------------
// <copyright file="ICurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// Contains all properties of an authenticated user.
    /// </summary>
    public interface ICurrentSession
    {
        AdminInfo AdminInfo { get; }

        LicenseeInfo LicenseeInfo { get; }

        AgentInfo AgentInfo { get; }

        int LoginUserId { get; }

        string LoginUserName { get; }

        AccountType LoginAccountType { get; }

        LoginType LoginType { get; }

        int OnUserId { get; }

        int OnLicenseeId { get; }

        string Language { get; set; }

        bool IsDirectSignIn { get; }

        bool IsValid { get; }

        bool IsAdmin { get; }

        bool IsCSDIT { get; }

        void UpdateAgent(AgentInfo agentInfo);
    }
}