// --------------------------------------------------------------------------
// <copyright file="AdminInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Entities
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class AdminInfo
    {
        public string UserName { get; set; }

        public string UserSalt { get; set; }

        public int UserId { get; set; }

        public string UserRole { get; set; }

        public int ServerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }
    }
}