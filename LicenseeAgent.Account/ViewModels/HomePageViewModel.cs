// -----------------------------------------------------------------------
// <copyright file="HomePageViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.ViewModels
{
    using LicenseeAgent.Account.Entities;

    public class HomePageViewModel
    {
        public StatisticEntity StatisticInfo { get; set; }

        public BalanceInfoEntity BalanceInfo { get; set; }
    }
}