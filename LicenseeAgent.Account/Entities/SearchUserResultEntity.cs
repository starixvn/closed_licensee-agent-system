// ----------------------------------------------------------------------
// <copyright file="SearchUserResultEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchUserResultEntity
    {
        public CustomerEntity Customer { get; set; }

        public IEnumerable<UplineEntity> Uplines { get; set; }

        public IEnumerable<DownlineEntity> Downlines { get; set; }
    }
}
