// -----------------------------------------------------------------------
// <copyright file="BetListWinlossViewModel.cs" company="Microsoft">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.BetList.ViewModels
{
    using System;

    public class BetListWinlossViewModel
    {
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string BreadCrumb { get; set; }

        public string DrawFilter { get; set; }

        public string StatusIDs { get; set; }

        public string Title { get; set; }
    }
}
