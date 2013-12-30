// --------------------------------------------------------------------------
// <copyright file="AdminInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Entities
{
    using System;

    /// <summary>
    /// CSD/IT sign-in information.
    /// </summary>
    [Serializable]
    public class AdminInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}