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
        /// Account type is admin.
        /// </summary>
        Admin = 1,

        /// <summary>
        /// Account type is Licensee.
        /// </summary>
        Licensee = 2,

        /// <summary>
        /// Account type is Agent.
        /// </summary>
        Agent = 3,

        /// <summary>
        /// Account type is Member.
        /// </summary>
        Member = 4
    }
}