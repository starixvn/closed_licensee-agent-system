// ----------------------------------------------------------------------
// <copyright file="MemberBalanceEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberBalanceEntity
    {
        public decimal TotalBalance { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal BetCredit { get; set; }
    }
}
