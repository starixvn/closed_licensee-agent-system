// ----------------------------------------------------------------------
// <copyright file="SearchUserResultViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.AccountInformation.Entities;
    using LicenseeAgent.Taiwan.Entities;

    public class SearchUserResultViewModel
    {
        public CustomerEntity Customer { get; set; }

        public IEnumerable<UplineEntity> Uplines { get; set; }

        public IEnumerable<DownlineEntity> Downlines { get; set; }

        public BalanceEntity Balance { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}