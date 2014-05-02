// -----------------------------------------------------------------------
// <copyright file="CustomerSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// CustomerSetting class.
    /// </summary>
    public class CustomerSetting
    {
        public CustomerInfo CustInfo { get; set; }

        public CommissionSetting CommissionSetting { get; set; }

        public IList<BettingLimitItem> BettingLimitSetting { get; set; }

        public IList<PositionTakingItem> PositionTakingSetting { get; set; }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>Returns true if entity is valid.</returns>
        public bool IsValid()
        {
            if (CommissionSetting == null
                    || CommissionSetting.CommissionItems == null
                    || CommissionSetting.CommissionItems.Count == 0
                    || BettingLimitSetting == null
                    || BettingLimitSetting.Count == 0
                    || PositionTakingSetting == null
                    || PositionTakingSetting.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}