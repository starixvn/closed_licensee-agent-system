// --------------------------------------------------------------------------
// <copyright file="BettingLimit.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    /// <summary>
    /// Class name.
    /// </summary>
    public class BettingLimit
    {
        public int SportId { get; set; }

        public int BetTypeId { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MinMaxBet { get; set; }

        public decimal MaxMinBet { get; set; }

        public decimal MinMaxPerMatch { get; set; }
    }
}