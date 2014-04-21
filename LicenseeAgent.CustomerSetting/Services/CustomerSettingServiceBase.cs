// --------------------------------------------------------------------------
// <copyright file="CustomerSettingServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Text;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerSetting.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// CustomerSettingServiceBase class.
    /// </summary>
    public class CustomerSettingServiceBase : QueryService
    {
        protected static JsonSerializerSettings GetJsonSetting
        {
            get
            {
                return new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented
                };
            }
        }

        /// <summary>
        /// Format JSON return to client.
        /// </summary>
        /// <typeparam name="T1">Bet Limit.</typeparam>
        /// <typeparam name="T2">Position Taking Group.</typeparam>
        /// <param name="customerSetting">The number game customer.</param>
        /// <param name="settingContext">Setting Context.</param>
        /// <returns>String JAVASCRIPT.</returns>
        public virtual StringBuilder FormatJavascript<T1, T2>(CustomerSetting<T1, T2> customerSetting, SettingContext settingContext = null)
        {
            var result = customerSetting != null ? customerSetting.Result : null;
            var custInfo = customerSetting != null ? customerSetting.CustInfo : null;
            var commissionSetting = customerSetting != null ? customerSetting.CommissionSetting : null;
            var betLimits = customerSetting != null ? customerSetting.BettingLimits : null;
            var positionTakingSetting = customerSetting != null ? customerSetting.PositionTakingSetting : null;

            JsonSerializerSettings jsonSetting = GetJsonSetting;
            var settingScript = new StringBuilder();
            if (result != null && result.ErrCode != 0)
            {
                settingScript.Append(string.Format("var result = {0}; \n", JsonConvert.SerializeObject(result, jsonSetting)));
            }
            else
            {
                if (settingContext != null)
                {
                    settingScript.Append(string.Format("var settingContext= {0}; \n", JsonConvert.SerializeObject(settingContext, jsonSetting)));
                }

                if (custInfo != null)
                {
                    settingScript.Append(string.Format("var custInfo= {0}; \n", JsonConvert.SerializeObject(custInfo, jsonSetting)));
                }

                if (commissionSetting != null)
                {
                    settingScript.Append(string.Format("var commissionSetting= {0}; \n", JsonConvert.SerializeObject(commissionSetting, jsonSetting)));
                }

                if (betLimits != null)
                {
                    settingScript.Append(string.Format("var betLimits= {0}; \n", JsonConvert.SerializeObject(betLimits, jsonSetting)));
                }

                if (positionTakingSetting != null)
                {
                    settingScript.Append(string.Format("var positionTakingSetting= {0}; \n", JsonConvert.SerializeObject(positionTakingSetting, jsonSetting)));
                }
            }

            return settingScript;
        }
    }
}