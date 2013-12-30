// --------------------------------------------------------------------------
// <copyright file="ICurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.6">Created</change>
// </history>

namespace LicenseeAgent.Web
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Contains all properties of an authenticated user.
    /// </summary>
    public interface ICurrentSession
    {
        AgentInfo AgentInfo { get; set; }

        AdminInfo AdminInfo { get; }

        InternalLoginInfo InternalLoginInfo { get; set; }

        int OnUserId { get; }

        string Language { get; set; }

        bool IsDirectSignIn { get; }

        string UserName { get; }

        int ServerId { get; }

        bool IsValid { get; }

        bool IsAdmin { get; }

        int LoginUserId { get; }
    }
}