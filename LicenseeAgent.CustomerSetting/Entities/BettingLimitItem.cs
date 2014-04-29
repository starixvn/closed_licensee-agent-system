// --------------------------------------------------------------------------
// <copyright file="BettingLimitItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    /// <summary>
    /// BettingLimitItem name.
    /// </summary>
    public class BettingLimitItem
    {
        public int SportId { get; set; }

        public int BetTypeId { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MinMaxBet { get; set; }

        public decimal MaxMinBet { get; set; }

        public decimal MinMaxPerMatch { get; set; }

        public decimal MaxPerBall { get; set; }

        public decimal MinMaxPerBall { get; set; }
    }
}