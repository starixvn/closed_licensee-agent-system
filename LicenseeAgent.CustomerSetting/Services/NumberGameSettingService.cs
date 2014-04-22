// ----------------------------------------------------------------------
// <copyright file="NumberGameSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Common;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Criteria.NumberGame.Agent;
    using LicenseeAgent.CustomerSetting.Criteria.NumberGame.Licensee;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.NumberGame;
    using LicenseeAgent.CustomerSetting.Repositories;
    using Newtonsoft.Json;

    /// <summary>
    /// NumberGameSettingService class.
    /// </summary>
    public class NumberGameSettingService : INumberGameSettingService
    {
        private readonly INumberGameSettingRepository _numberGameSettingRepository;

        public NumberGameSettingService(INumberGameSettingRepository numberGameSettingRepository)
        {
            _numberGameSettingRepository = numberGameSettingRepository;
        }

        /// <summary>
        /// Get Agent setting for insert new.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <returns>Number game customer setting.</returns>
        public CustomerSetting GetNumberGameSettingForInsert(SettingContext settingContext)
        {
            CommissionSetting commissionSetting;
            IList<NumberGameBettingLimit> bettingLimits;
            Dictionary<string, PositionTaking> positionTakingData;

            ICriteria getAgentSettingCriteria = GetInsertSettingCriteria(settingContext);

            var msgId = 0;
            var maxLicenseePt = 0;
            _numberGameSettingRepository.GetSettingForInsert(
                getAgentSettingCriteria,
                settingContext,
                out commissionSetting,
                out bettingLimits,
                out positionTakingData,
                out msgId,
                out maxLicenseePt);

            if (msgId == 0)
            {
                return new CustomerSetting
                {
                    CommissionSetting = commissionSetting,
                    BettingLimits = bettingLimits.Cast<BettingLimit>().ToList(),
                    PositionTakingSetting = ServiceUtils.BuildPositionTakingGroups((int)Product.Product_2, settingContext, positionTakingData, maxLicenseePt)
                };
            }

            return new CustomerSetting
            {
                Result = new Result(msgId, ResourceManager.GetErrMsg(msgId))
            };
        }

        public StringBuilder GetFormatJavascript(CustomerSetting numberGameCustomer)
        {
            return FormatJavascript(numberGameCustomer);
        }

        /// <summary>
        /// Format JSON return to client.
        /// </summary>
        /// <param name="customerSetting">The number game customer.</param>
        /// <param name="settingContext">Setting Context.</param>
        /// <returns>String JAVASCRIPT.</returns>
        public StringBuilder FormatJavascript(
            CustomerSetting customerSetting,
            SettingContext settingContext = null)
        {
            var settingScript = new StringBuilder();

            var commissionSetting = customerSetting != null ? customerSetting.CommissionSetting : null;
            var betLimits = customerSetting != null ? customerSetting.BettingLimits : null;
            var positionTakingSetting = customerSetting != null ? customerSetting.PositionTakingSetting : null;
            JsonSerializerSettings jsonSetting = ServiceUtils.GetJsonSetting;

            if (commissionSetting != null)
            {
                settingScript.Append(string.Format("var numberGameCommissionSetting= {0}; \n", JsonConvert.SerializeObject(commissionSetting, jsonSetting)));
            }

            if (betLimits != null)
            {
                settingScript.Append(string.Format("var numberGameBetLimits= {0}; \n", JsonConvert.SerializeObject(betLimits, jsonSetting)));
            }

            if (positionTakingSetting != null)
            {
                settingScript.Append(string.Format("var numberGamePositionTakingSetting= {0}; \n", JsonConvert.SerializeObject(positionTakingSetting, jsonSetting)));
            }

            return settingScript;
        }

        /// <summary>
        /// Get Insert Setting Criteria.
        /// </summary>
        /// <param name="settingContext">Setting Context.</param>
        /// <returns>Insert Criteria.</returns>
        private static ICriteria GetInsertSettingCriteria(SettingContext settingContext)
        {
            if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                return new LicenseeGetSettingForInsertCriteria
                {
                    AgentL0Id = settingContext.CustId,
                };
            }

            return new AgentGetSettingForInsertCriteria
            {
                AgentId = settingContext.CustId
            };
        }
    }
}