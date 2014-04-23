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
        public static IList<Entities.NumberGame.BettingLimit> ReadNumberGameBetLimitData(
            IDataReader reader, 
            bool getUplineBetlimit = false)
        {
            var bettingLimits = new List<Entities.NumberGame.BettingLimit>();
            while (reader.Read())
            {
                var sportType = reader.ToInt("SportType");
                var bettingLimit = new Entities.NumberGame.BettingLimit
                {
                    SportId = sportType,
                    BetTypeId = reader.ToInt("BetType"),
                    MinBet = reader.ToDecimal("MinBet"),
                    MaxBet = reader.ToDecimal("MaxBet"),
                    MaxPerMatch = reader.ToDecimal("MaxPerMatch"),
                    MaxPerBall = reader.ToDecimal("MaxPerBall")
                };

                bettingLimits.Add(bettingLimit);
            }

            bettingLimits.OrderBy(bt => bt.SportId).OrderBy(bt => bt.BetTypeId).ToList();
            return bettingLimits;
        }

        /// <summary>
        /// Read bet limits setting.
        /// </summary>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineBetlimit">Get up line bet limits or not.</param>
        /// <returns>Returns list of betting limit item.</returns>
        public static IList<BettingLimit> ReadBetLimitData(
            IDataReader reader, 
            bool getUplineBetlimit = false)
        {
            var bettingLimits = new List<BettingLimit>();
            while (reader.Read())
            {
                var sportType = reader.ToInt("SportType");
                var bettingLimit = new BettingLimit
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

            bettingLimits.OrderBy(bt => bt.SportId).OrderBy(bt => bt.BetTypeId).ToList();
            return bettingLimits;
        }
    }
}