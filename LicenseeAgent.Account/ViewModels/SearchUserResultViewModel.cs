// ----------------------------------------------------------------------
// <copyright file="SearchUserResultViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Account.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchUserResultViewModel
    {
        public CustomerEntity Customer { get; set; }

        public IEnumerable<UplineEntity> Uplines { get; set; }

        public IEnumerable<DownlineEntity> Downlines { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}
