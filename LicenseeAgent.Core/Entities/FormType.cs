// --------------------------------------------------------------------------
// <copyright file="FormType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum FormType : int
    {
        /// <summary>
        /// Add direct agent or member.
        /// </summary>
        Add = 0,

        /// <summary>
        /// Edit direct agent or member.
        /// </summary>
        Edit = 1,

        /// <summary>
        /// View indirect agent.
        /// </summary>
        ViewIndirectAgent = 2,

        /// <summary>
        /// Edit indirect member.
        /// </summary>
        EditIndirectMember = 3,

        /// <summary>
        /// Edit multiple member.
        /// </summary>
        EditMultiple = 4
    }
}