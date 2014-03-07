// ----------------------------------------------------------------------
// <copyright file="BalanceEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BalanceEntity
    {
        public decimal TotalCash { get; set; }

        public decimal YesterdayCash { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal TotalOutstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal UsedCredit { get; set; }

        public decimal BetCredit { get; set; }
    }
}
