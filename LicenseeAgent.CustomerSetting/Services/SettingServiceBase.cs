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

            if (!customerSetting.IsValid(product, settingContext, true))
            {
                errCode = Result.InvalidDbData;
            }

            var result = new CustomerSetting2
            {
                CustInfo = customerSetting.CustInfo,
                CommissionSetting = customerSetting.CommissionSetting,
                BettingLimitSetting = customerSetting.BettingLimitSetting,
                PositionTakingSetting = PositionTakingBuilder.BuildGroups((int)product, settingContext, customerSetting.PositionTakingSetting, maxLicenseePt)
            };

            return result;
        }
    }
}