// --------------------------------------------------------------------------
// <copyright file="LoginInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.API.LRF.Entities
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class LoginInfo
    {
        public LoginInfo()
        {
            Timeout = DateTime.Now.AddMinutes(30);
        }

        public int UserId { get; set; }

        public int OnUserId { get; set; }

        public int LicenseeId { get; set; }

        public string Language { get; set; }

        public DateTime Timeout { get; private set; }

        public bool IsExpired
        {
            get
            {
                return DateTime.Now > Timeout;
            }
        }
    }
}