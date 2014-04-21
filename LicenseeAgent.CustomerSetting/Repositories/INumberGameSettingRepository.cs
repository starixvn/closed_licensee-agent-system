// ----------------------------------------------------------------------
// <copyright file="INumberGameSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.NumberGame;

    /// <summary>
    /// INumberGameSettingRepository interface.
    /// </summary>
    public interface INumberGameSettingRepository
    {
        /// <summary>
        /// Get default setting for insert new agent.
        /// </summary>
        /// <param name="criteria">SP parameters.</param>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="commissionSetting">Commission setting.</param>
        /// <param name="bettingLimits">Bet limit setting.</param>
        /// <param name="positionTakingDataList">Position taking setting.</param>
        /// <param name="msgId">The MSG unique identifier.</param>
        /// <param name="maxLicenseePt">Max PT in case insert agent level 1.</param>
        /// <exception cref="System.Exception">Throw an invalid criteria exception.</exception>
        void GetSettingForInsert(
            ICriteria criteria,
            SettingContext settingContext,
            out CommissionSetting commissionSetting,
            out IList<BettingLimit> bettingLimits,
            out Dictionary<string, PositionTaking> positionTakingDataList,
            out int msgId,
            out int maxLicenseePt);
    }
}
