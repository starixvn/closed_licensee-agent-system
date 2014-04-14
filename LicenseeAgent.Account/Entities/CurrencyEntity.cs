// --------------------------------------------------------------------------
// <copyright file="CurrencyEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.10.21">Created</change>
// </history>

namespace LicenseeAgent.Account.Entities
{
    public class CurrencyEntity
    {
        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public byte NumberAgentLevel { get; set; }
    }
}