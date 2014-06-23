// -----------------------------------------------------------------------
// <copyright file="StatisticEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    /// <summary>
    /// StatisticEntity class.
    /// </summary>
    public class StatisticEntity
    {
        public decimal OutStanding { get; set; }

        public decimal TotalOutStanding { get; set; }

        public int NumberNewMembers { get; set; }
    }
}