// --------------------------------------------------------------------------
// <copyright file="Statement.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Entities
{
    using System;
    using LicenseeAgent.Taiwan.Attributes;

    /// <summary>
    /// Statement Entity.
    /// </summary>
    public class Statement
    {
        [Service(Name = "IBCCustId")]
        public int CustId { get; set; }

        [Service(Name = "LASAgentId")]
        public int AgentId { get; set; }

        public DateTime Date { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal Turnover { get; set; }

        public decimal Betting { get; set; }

        public decimal Winloss { get; set; }

        public decimal Outstanding { get; set; }

        public decimal Commission { get; set; }

        public decimal TransferAmt { get; set; }

        public decimal ClosingBalance { get; set; }
    }
}