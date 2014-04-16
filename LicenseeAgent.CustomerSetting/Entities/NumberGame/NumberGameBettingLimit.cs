// -----------------------------------------------------------------------
// <copyright file="NumberGameBettingLimit.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities.NumberGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NumberGameBettingLimit
    {
        public int SportId { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MinMaxPerMatch { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MinMaxBet { get; set; }

        public decimal MaxMinBet { get; set; }

        public decimal MaxPerBall { get; set; }
    }
}
