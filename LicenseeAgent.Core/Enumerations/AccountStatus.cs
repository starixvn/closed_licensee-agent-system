// --------------------------------------------------------------------------
// <copyright file="AccountStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Enumerations
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum AccountStatus : byte
    {
        /// <summary>
        /// The account is closed.
        /// </summary>
        Closed = 0,

        /// <summary>
        /// The account is open.
        /// </summary>
        Open = 1
    }
}