// -----------------------------------------------------------------------
// <copyright file="NewMemberOfMonthEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    using System;

    public class NewMemberOfMonthEntity
    {
        public string Username { get; set; }

        public decimal Credit { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}