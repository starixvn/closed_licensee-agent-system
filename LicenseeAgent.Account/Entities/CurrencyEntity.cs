// --------------------------------------------------------------------------
// <copyright file="CurrencyEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    /// <summary>
    /// CurrencyEntity class.
    /// </summary>
    public class CurrencyEntity
    {
        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public byte NumberAgentLevel { get; set; }
    }
}