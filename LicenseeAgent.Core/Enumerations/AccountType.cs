// --------------------------------------------------------------------------
// <copyright file="AccountType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Enumerations
{
    /// <summary>
    /// List of account types in LAS system.
    /// </summary>
    public enum AccountType : byte
    {
        /// <summary>
        /// Account type is CSD/IT or Licensee.
        /// </summary>
        Admin = 1,

        /// <summary>
        /// Account type is CSD/IT.
        /// </summary>
        CSD_IT = 2,

        /// <summary>
        /// Account type is Licensee.
        /// </summary>
        Licensee = 3,

        /// <summary>
        /// Account type is Agent (any levels).
        /// </summary>
        Agent = 4,

        /// <summary>
        /// The agent level is 0.
        /// </summary>
        AgentLevel0 = 5,

        /// <summary>
        /// The agent level is 1.
        /// </summary>
        AgentLevel1 = 6,

        /// <summary>
        /// The agent level is k.
        /// </summary>
        AgentLevelk = 7,

        /// <summary>
        /// The agent level is n.
        /// </summary>
        AgentLeveln = 8,

        /// <summary>
        /// Account type is Member.
        /// </summary>
        Member = 9
    }
}