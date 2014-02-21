// --------------------------------------------------------------------------
// <copyright file="Statement.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.2.21">Created</change>
// </history>

namespace LicenseeAgent.Report.Entities
{
    using System;

    /// <summary>
    /// Statement Entity.
    /// </summary>
    public class Statement
    {
        public int CustID { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal Turnover { get; set; }

        public decimal Betting { get; set; }

        public decimal Outstanding { get; set; }

        public decimal Commission { get; set; }

        public decimal TransferAmt { get; set; }

        public decimal ClosingBalance { get; set; }
    }
}
