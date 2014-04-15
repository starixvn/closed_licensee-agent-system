// --------------------------------------------------------------------------
// <copyright file="UpdateDataSportsBookCustomer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities.SportsBook
{
    using System.Collections.Generic;

    /// <summary>
    /// UpdateDataSportsBookCustomer class.
    /// </summary>
    public class UpdateDataSportsBookCustomer
    {
        public CustomerInfo CustInfo { get; set; }

        public CommissionSetting CommissionSetting { get; set; }

        public IList<BettingLimit> BettingLimits { get; set; }

        public IList<PositionTaking> PositionTakingSetting { get; set; }
    }
}