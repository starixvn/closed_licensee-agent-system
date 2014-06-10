// --------------------------------------------------------------------------
// <copyright file="BettingLimitDataReader.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Utils
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common.Extensions;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BettingLimitDataReader
    {
        /// <summary>
        /// Read bet limits setting.
        /// </summary>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineBetlimit">Get up line bet limits or not.</param>
        /// <returns>Returns list of Betting limit item.</returns>
        public static IList<BettingLimitItem> ReadNumberGameBetLimitData(
            IDataReader reader,
            bool getUplineBetlimit = false)
        {
            var bettingLimits = new List<BettingLimitItem>();
            while (reader.Read())
            {
                var sportType = reader.ToInt("SportType");
                var bettingLimit = new BettingLimitItem
                {
                    SportId = sportType,
                    BetTypeId = reader.ToInt("BetType"),
                    MinBet = reader.ToDecimal("MinBet"),
                    MaxBet = reader.ToDecimal("MaxBet"),
                    MaxPerBall = reader.ToDecimal("MaxPerBall"),
                    MaxPerMatch = reader.ToDecimal("MaxPerMatch")
                };

                if (getUplineBetlimit)
                {
                    bettingLimit.MaxMinBet = reader.ToDecimal("MaxMinBet");
                    bettingLimit.MinMaxBet = reader.ToDecimal("MinMaxBet");
                    bettingLimit.MinMaxPerBall = reader.ToDecimal("MinMaxPerBall");
                    bettingLimit.MinMaxPerMatch = reader.ToDecimal("MinMaxPerMatch");
                }
                else
                {
                    bettingLimit.MaxMinBet = bettingLimit.MinBet;
                    bettingLimit.MinMaxBet = bettingLimit.MaxBet;
                    bettingLimit.MinMaxPerBall = bettingLimit.MaxPerBall;
                    bettingLimit.MinMaxPerMatch = bettingLimit.MaxPerMatch;
                }

                bettingLimits.Add(bettingLimit);
            }

            return bettingLimits.OrderBy(bt => bt.SportId).OrderBy(bt => bt.BetTypeId).ToList();
        }

        /// <summary>
        /// Read bet limits setting.
        /// </summary>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineBetlimit">Get up line bet limits or not.</param>
        /// <returns>Returns list of betting limit item.</returns>
        public static IList<BettingLimitItem> ReadBetLimitData(
            IDataReader reader,
            bool getUplineBetlimit = false)
        {
            var bettingLimits = new List<BettingLimitItem>();
            while (reader.Read())
            {
                var sportType = reader.ToInt("SportType");
                var bettingLimit = new BettingLimitItem
                {
                    SportId = sportType,
                    BetTypeId = reader.ToInt("BetType"),
                    MinBet = reader.ToDecimal("MinBet"),
                    MaxBet = reader.ToDecimal("MaxBet"),
                    MaxPerMatch = reader.ToDecimal("MaxPerMatch")
                };

                if (getUplineBetlimit)
                {
                    bettingLimit.MaxMinBet = reader.ToDecimal("MaxMinBet");
                    bettingLimit.MinMaxBet = reader.ToDecimal("MinMaxBet");
                    bettingLimit.MinMaxPerMatch = reader.ToDecimal("MinMaxPerMatch");
                }
                else
                {
                    bettingLimit.MaxMinBet = bettingLimit.MinBet;
                    bettingLimit.MinMaxBet = bettingLimit.MaxBet;
                    bettingLimit.MinMaxPerMatch = bettingLimit.MaxPerMatch;
                }

                bettingLimits.Add(bettingLimit);
            }

            return bettingLimits.OrderBy(bt => bt.SportId).OrderBy(bt => bt.BetTypeId).ToList();
        }
    }
}