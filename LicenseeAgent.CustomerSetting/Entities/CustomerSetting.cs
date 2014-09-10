namespace LicenseeAgent.CustomerSetting.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Utils;

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
        /// <param name="product">The product id.</param>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="isGet">If set to <c>true</c> mean that settings is get from DB.</param>
        /// <returns>
        /// Returns true if entity is valid.
        /// </returns>
        public bool IsValid(Product product, SettingContext settingContext, bool isGet = false)
        {
            if (!ValidateBetSetting(product, settingContext))
            {
                return false;
            }

            if (settingContext.IsCredit && !ValidateCommissionSetting(product, settingContext))
            {
                return false;
            }

            if (!ValidatePositionTakingSetting(product, settingContext, isGet))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the bet setting.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Returns true if valid.</returns>
        private bool ValidateBetSetting(Product product, SettingContext settingContext)
        {
            if (settingContext.FormId == FormType.Add)
            {
                if (BettingLimitSetting == null
                    || BettingLimitSetting.Count == 0)
                {
                    return false;
                }
            }

            if (BettingLimitSetting != null)
            {
                var productSettings = SettingConfiguration.GetProductPtSettings((int)product);
                foreach (var item in BettingLimitSetting)
                {
                    if (!productSettings.SportType.Where(x => x.Id == item.SportId && x.BetTypeIds.Contains(item.BetTypeId.ToString())).Any())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Validates the commission setting.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Returns true if valid.</returns>
        private bool ValidateCommissionSetting(Product product, SettingContext settingContext)
        {
            if (product == Product.Product_5)
            {
                return true;
            }

            if (settingContext.FormId == FormType.Add)
            {
                if (CommissionSetting == null
                    || CommissionSetting.CommissionItems == null
                    || CommissionSetting.CommissionItems.Count == 0)
                {
                    return false;
                }
            }

            if (CommissionSetting != null && CommissionSetting.CommissionItems != null)
            {
                var productSettings = SettingConfiguration.GetProductPtSettings((int)product);
                foreach (var item in CommissionSetting.CommissionItems)
                {
                    if (!productSettings.SportType.Where(x => x.Id == item.SportId && x.BetTypeIds.Contains(item.BetTypeId.ToString())).Any())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Validates the position taking setting.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="isGet">If set to <c>true</c> [is strict].</param>
        /// <returns>Returns true if valid.</returns>
        private bool ValidatePositionTakingSetting(Product product, SettingContext settingContext, bool isGet)
        {
            if (settingContext.FormId == FormType.Add)
            {
                if (settingContext.RoleId == (int)Role.AgentLevel0 && isGet)
                {
                    return true;
                }
            }

            if ((settingContext.FormId == FormType.Add || isGet)
                && (PositionTakingSetting == null || PositionTakingSetting.Count == 0))
            {
                return false;
            }

            if (PositionTakingSetting != null)
            {
                var configKeys = new List<string>();
                var productSettings = SettingConfiguration.GetProductPtSettings((int)product);
                foreach (var sportType in productSettings.SportType)
                {
                    var betTypeIds = sportType.BetTypeIds.Split(',');
                    foreach (var betTypeId in betTypeIds)
                    {
                        configKeys.Add(PositionTakingBuilder.GenerateKey(sportType.Id, sportType.IsLive, Convert.ToInt32(betTypeId)));
                    }
                }

                var inputKeys = new List<string>();
                foreach (var item in PositionTakingSetting)
                {
                    inputKeys.Add(PositionTakingBuilder.GenerateKey(item.SportId, item.IsLive, item.BetTypeId));
                }

                if ((settingContext.FormId == FormType.Add || isGet) && inputKeys.Count != configKeys.Count)
                {
                    return false;
                }

                foreach (var key in inputKeys)
                {
                    if (!configKeys.Contains(key))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}