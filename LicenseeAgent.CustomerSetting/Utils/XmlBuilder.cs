// ----------------------------------------------------------------------
// <copyright file="XmlBuilder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class XmlBuilder
    {
        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="commissionSetting">Commission setting data.</param>
        /// <returns>String data for update to database.</returns>
        public static string Build(SettingContext settingContext, CommissionSetting commissionSetting)
        {
            string xmlStringData = string.Empty;
            if (commissionSetting != null
                && commissionSetting.CommissionItems != null
                && commissionSetting.CommissionItems.Any())
            {
                string tempString;
                if (settingContext.TargetRoleId != (int)Role.Member)
                {
                    foreach (CommissionItem comItem in commissionSetting.CommissionItems)
                    {
                        tempString = string.Format(
                            Constant.FORMAT_AGENT_COMMISSION_SETTING_XML,
                            comItem.SportId,
                            comItem.BetTypeId,
                            comItem.DiscountA / Constant.STEP_MUL_DIV,
                            comItem.DiscountB / Constant.STEP_MUL_DIV,
                            comItem.DiscountC / Constant.STEP_MUL_DIV,
                            comItem.DiscountD / Constant.STEP_MUL_DIV);

                        xmlStringData += tempString;
                    }
                }
                else
                {
                    foreach (CommissionItem comItem in commissionSetting.CommissionItems)
                    {
                        tempString = string.Format(
                            Constant.FORMAT_MEMBER_COMMISSION_SETTING_XML,
                            comItem.SportId,
                            comItem.BetTypeId,
                            comItem.Discount / Constant.STEP_MUL_DIV);

                        xmlStringData += tempString;
                    }
                }
            }

            return !string.IsNullOrEmpty(xmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, xmlStringData)
                : string.Empty;
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="bettingLimits">Betting limits data.</param>
        /// <returns>String data to update database.</returns>
        public static string Build(IList<BettingLimitItem> bettingLimits)
        {
            string xmlStringData = string.Empty;
            if (bettingLimits != null && bettingLimits.Any())
            {
                foreach (BettingLimitItem betLimit in bettingLimits)
                {
                    xmlStringData += string.Format(
                        Constant.FORMAT_BETTING_LIMIT_XML,
                        betLimit.SportId,
                        betLimit.BetTypeId,
                        betLimit.MinBet,
                        betLimit.MaxBet,
                        betLimit.MaxPerMatch);
                }
            }

            return !string.IsNullOrEmpty(xmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, xmlStringData)
                : string.Empty;
        }

        /// <summary>
        /// Builds for number game.
        /// </summary>
        /// <param name="bettingLimits">The betting limits.</param>
        /// <returns>String data to update database.</returns>
        public static string BuildForNumberGame(IList<BettingLimitItem> bettingLimits)
        {
            string xmlStringData = string.Empty;
            if (bettingLimits != null && bettingLimits.Any())
            {
                foreach (BettingLimitItem betLimit in bettingLimits)
                {
                    xmlStringData += string.Format(
                        Constant.FORMAT_BETTING_LIMIT_XML_3RD,
                        betLimit.SportId,
                        betLimit.BetTypeId,
                        betLimit.MinBet,
                        betLimit.MaxBet,
                        betLimit.MaxPerMatch,
                        betLimit.MaxPerBall);
                }
            }

            return !string.IsNullOrEmpty(xmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, xmlStringData)
                : string.Empty;
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingSetting">Position taking data.</param>
        /// <returns>String data to update database.</returns>
        public static string Build(SettingContext settingContext, IList<PositionTakingItem> positionTakingSetting)
        {
            string xmlStringData = string.Empty;

            if (positionTakingSetting == null || !positionTakingSetting.Any())
            {
                return xmlStringData;
            }

            foreach (PositionTakingItem positionTaking in positionTakingSetting)
            {
                if (positionTaking != null)
                {
                    xmlStringData += Build(settingContext, positionTaking);
                }
            }

            return !string.IsNullOrEmpty(xmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, xmlStringData)
                : string.Empty;
        }

        /// <summary>
        /// Builds the XML for one position taking setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="positionTaking">The position taking.</param>
        /// <returns>The xml string for one position taking item.</returns>
        private static string Build(SettingContext settingContext, PositionTakingItem positionTaking)
        {
            string xmlResult;

            if (settingContext.TargetRoleId != (int)Role.Member)
            {
                if (settingContext.TargetRoleId == (int)Role.Agent && settingContext.FormId == FormType.Edit)
                {
                    xmlResult = string.Format(
                            Constant.FORMAT_AGENT_EDIT_AGENT_POSITION_TAKING_XML,
                            positionTaking.SportId,
                            positionTaking.BetTypeId,
                            positionTaking.IsLive ? 1 : 0,
                            (int)(positionTaking.Preset * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Max * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Min * Constant.STEP_MUL_DIV),
                            positionTaking.ApplyAll ? 1 : 0);
                }
                else
                {
                    xmlResult = string.Format(
                            Constant.FORMAT_AGENT_POSITION_TAKING_XML,
                            positionTaking.SportId,
                            positionTaking.BetTypeId,
                            positionTaking.IsLive ? 1 : 0,
                            (int)(positionTaking.Preset * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Max * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Min * Constant.STEP_MUL_DIV));
                }
            }
            else
            {
                xmlResult = string.Format(
                        Constant.FORMAT_MEMBER_POSITION_TAKING_XML,
                        positionTaking.SportId,
                        positionTaking.BetTypeId,
                        positionTaking.IsLive ? 1 : 0,
                        (int)(positionTaking.Value * Constant.STEP_MUL_DIV));
            }

            return xmlResult;
        }
    }
}