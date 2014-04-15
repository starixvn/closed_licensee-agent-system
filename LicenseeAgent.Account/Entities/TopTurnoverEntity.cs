// -----------------------------------------------------------------------
// <copyright file="TopTurnoverEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    /// <summary>
    /// TopTurnoverEntity class.
    /// </summary>
    public class TopTurnoverEntity
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal Turnover { get; set; }
    }
}
