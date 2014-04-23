// ----------------------------------------------------------------------
// <copyright file="CustomerSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities.NumberGame
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerSetting
    {
        public CommissionSetting CommissionSetting { get; set; }

        public IList<BettingLimit> BettingLimits { get; set; }

        public IList<PositionTakingGroup> PositionTakingSetting { get; set; }
    }
}