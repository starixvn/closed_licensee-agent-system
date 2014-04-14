// --------------------------------------------------------------------------
// <copyright file="MemberDetail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Entities
{
    using System;

    public class MemberDetail
    {
        public string UserName { get; set; }

        public decimal BetCredit { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime LastTransaction { get; set; }
    }
}