// -----------------------------------------------------------------------
// <copyright file="CustomerSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// CustomerSetting class.
    /// </summary>
    public class CustomerSetting<T1, T2>
    {
        public Result Result { get; set; }

        public CustomerInfo CustInfo { get; set; }

        public CommissionSetting CommissionSetting { get; set; }

        public IList<T1> BettingLimits { get; set; }

        public IList<T2> PositionTakingSetting { get; set; }
    }
}
