// --------------------------------------------------------------------------
// <copyright file="SportsBookCustomer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities.SportsBook
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// SportsBookCustomer class.
    /// </summary>
    public class SportsBookCustomer
    {
        public Result Result { get; set; }

        public CustomerInfo CustInfo { get; set; }

        public CommissionSetting CommissionSetting { get; set; }

        public IList<BettingLimit> BettingLimits { get; set; }

        public IList<PositionTakingGroup> PositionTakingSetting { get; set; }
    }
}