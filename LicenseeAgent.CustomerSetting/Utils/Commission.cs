namespace LicenseeAgent.CustomerSetting.Utils
{
    using System;
    using System.Globalization;
    using LicenseeAgent.CustomerSetting.Core;

    /// <summary>
    /// Commission class.
    /// </summary>
    public static class Commission
    {
        /// <summary>
        /// Combine commission.
        /// </summary>
        /// <param name="commission1">Commission One.</param>
        /// <param name="commission2">Commission Two.</param>
        /// <param name="commission3">Commission Three.</param>
        /// <returns>Commission after combine.</returns>
        public static decimal Combine(int commission1, int commission2, int commission3)
        {
            var cultureInfo = CultureInfo.GetCultureInfo("en-US");
            return Convert.ToDecimal(string.Format(cultureInfo, Constant.FormatCommission, commission1, commission2, commission3)) / Constant.StepMulDiv;
        }
    }
}
