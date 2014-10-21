namespace LicenseeAgent.ProductSetting.Services.ProductSetting
{
    using System.Diagnostics.CodeAnalysis;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.Utils;

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
        /// <param name="result">The error code.</param>
        /// <param name="maxLicenseePT">The maximum licensee PT.</param>
        /// <returns>Returns customer setting 2.</returns>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Verified.")]
        public CustomerSetting2 BuildCustomerSetting2(
            ProductCode product,
            CustomerSetting customerSetting,
            SettingContext settingContext,
            Result result,
            int maxLicenseePT = 0)
        {
            if (result.ErrCode != Result.SuccessValue)
            {
                return null;
            }

            if (!customerSetting.IsValid(product, settingContext, true))
            {
                result.ErrCode = Result.InvalidDbData;
            }

            return new CustomerSetting2
            {
                CustInfo = customerSetting.CustInfo,
                CommissionSetting = customerSetting.CommissionSetting,
                BettingLimitSetting = customerSetting.BettingLimitSetting,
                PositionTakingSetting = PositionTakingBuilder.BuildGroups((int)product, settingContext, customerSetting.PositionTakingSetting, maxLicenseePT)
            };
        }
    }
}