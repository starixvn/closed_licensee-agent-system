// -----------------------------------------------------------------------
// <copyright file="CustomerSetting2.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// CustomerSetting2 class.
    /// </summary>
    public class CustomerSetting2
    {
        public CustomerInfo CustInfo { get; set; }

        public CommissionSetting CommissionSetting { get; set; }

        public IList<BettingLimitItem> BettingLimitSetting { get; set; }

        public IList<PositionTakingGroup> PositionTakingSetting { get; set; }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <param name="product">The product id.</param>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>
        /// Returns true if entity is valid.
        /// </returns>
        public bool IsValid(Product product, SettingContext settingContext)
        {
            if (settingContext.FormId == FormType.Add)
            {
                if (product != Product.Product_5)
                {
                    if (CommissionSetting == null
                        || CommissionSetting.CommissionItems == null
                        || CommissionSetting.CommissionItems.Count == 0)
                    {
                        return false;
                    }
                }

                if (BettingLimitSetting == null
                    || BettingLimitSetting.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}