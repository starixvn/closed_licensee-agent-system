// --------------------------------------------------------------------------
// <copyright file="LicenseeInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Entities
{
    using System;

    /// <summary>
    /// Licensee user information.
    /// </summary>
    [Serializable]
    public class LicenseeInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int LicenseeId { get; set; }
    }
}