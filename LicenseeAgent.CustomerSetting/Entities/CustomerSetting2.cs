// -----------------------------------------------------------------------
// <copyright file="CustomerSetting2.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// CustomerSetting class.
    /// </summary>
    public class CustomerSetting2
    {
        public CustomerInfo CustInfo { get; set; }

        public CommissionSetting CommissionSetting { get; set; }

        public IList<BettingLimit> BettingLimits { get; set; }

        public IList<PositionTakingGroup> PositionTakingSetting { get; set; }
    }
}