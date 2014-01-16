// --------------------------------------------------------------------------
// <copyright file="AgentBalanceInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentBalanceInfo
    {
        public long RowNo { get; set; }

        public int AgentId { get; set; }

        public string UserName { get; set; }

        public string CustomerName { get; set; }

        public bool Status { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal YesterdayCash { get; set; }

        public decimal TotalCash { get; set; }

        public decimal Outstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal UsedCredit { get; set; }
    }
}