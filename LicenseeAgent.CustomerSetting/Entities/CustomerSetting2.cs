// -----------------------------------------------------------------------
// <copyright file="CustomerSetting2.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Core;

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
                var productSettings = SettingConfigurations.GetProductPtSettings((int)product);
                if (CommissionSetting == null
                    || CommissionSetting.CommissionItems == null
                    || CommissionSetting.CommissionItems.Count == 0
                    || BettingLimitSetting == null
                    || BettingLimitSetting.Count == 0)
                {
                    return false;
                }

                if (PositionTakingSetting.Where(x => !productSettings.SportType.Select(item => item.Id).Contains(x.SportId)).ToList().Any())
                {
                    return false;
                }

                foreach (var sportType in productSettings.SportType)
                {
                    var betTypes = sportType.BetTypeIds.Split(',');
                    var sport = PositionTakingSetting.Where(
                        x => x.SportId == sportType.Id &&
                        x.IsLive == sportType.IsLive &&
                        x.PositionTakings.Count == betTypes.Count()).ToList().FirstOrDefault();

                    if (sport.PositionTakings.Where(x => !betTypes.Contains(x.BetTypeId.ToString())).ToList().Any())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}