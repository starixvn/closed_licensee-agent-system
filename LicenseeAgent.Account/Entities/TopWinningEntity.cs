// -----------------------------------------------------------------------
// <copyright file="TopWinningEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    public class TopWinningEntity
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal WinLost { get; set; }
    }
}