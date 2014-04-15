// --------------------------------------------------------------------------
// <copyright file="Transfer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Entities
{
    using System;

    /// <summary>
    /// Transfer Entity.
    /// </summary>
    public class Transfer
    {
        public DateTime Date { get; set; }

        public string TransType { get; set; }

        public string TransDesc { get; set; }

        public decimal TransferAmt { get; set; }
    }
}
