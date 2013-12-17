// -----------------------------------------------------------------------
// <copyright file="IpEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IpEntity
    {
        public short IpKeyFrom { get; set; }

        public short IpKeyTo { get; set; }

        public long IpFrom { get; set; }

        public long IpTo { get; set; }

        public string CountryShort { get; set; }

        public string CountryLong { get; set; }

        public string IpRegion { get; set; }

        public string IpCity { get; set; }

        public bool IsModified { get; set; }

        public bool IsException { get; set; }
    }
}
