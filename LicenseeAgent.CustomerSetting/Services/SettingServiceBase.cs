// --------------------------------------------------------------------------
// <copyright file="SettingServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class SettingServiceBase
    {
        /// <summary>
        /// Builds the customer settings2.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="errCode">The error code.</param>
        /// <param name="maxLicenseePt">The maximum licensee PT.</param>
        /// <returns>Returns customer setting 2.</returns>
        public CustomerSetting2 BuildCustomerSetting2(
            Product product,
            CustomerSetting customerSetting,
            SettingContext settingContext,
            ref int errCode,
            int maxLicenseePt = 0)
        {
            if (errCode != Result.SuccessValue)
            {
                return null;
            }

            if (!customerSetting.IsValid(product, settingContext))
            {
                errCode = Result.FailureValue;
                return null;
            }

            var result = new CustomerSetting2
            {
                CustInfo = customerSetting.CustInfo,
                CommissionSetting = customerSetting.CommissionSetting,
                BettingLimitSetting = customerSetting.BettingLimitSetting,
                PositionTakingSetting = PositionTakingBuilder.BuildGroups((int)product, settingContext, customerSetting.PositionTakingSetting, maxLicenseePt)
            };

            if (!result.IsValid(product, settingContext))
            {
                errCode = Result.FailureValue;
                return null;
            }

            return result;
        }
    }
}