// --------------------------------------------------------------------------
// <copyright file="InternalLoginInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Web.Entities
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class InternalLoginInfo
    {
        public int AdminId { get; set; }

        public int OnAdminId { get; set; }

        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string Language { get; set; }
    }
}