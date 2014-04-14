// -----------------------------------------------------------------------
// <copyright file="DownlineRole.cs" company="Nexcel">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web.Enumerations
{
    public enum DownLineRole : int
    {
        /// <summary>
        /// None value.
        /// </summary>
        None = 0,

        /// <summary>
        /// Check agent belongs to agent.
        /// </summary>
        Agent = 1,

        /// <summary>
        /// Check member belongs to agent.
        /// </summary>
        Member = 2
    }
}