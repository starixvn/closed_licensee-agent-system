// --------------------------------------------------------------------------
// <copyright file="MemberBalanceInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Entities
{
    using LicenseeAgent.Taiwan;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberBalanceInfo
    {
        public long RowNo { get; set; }

        [Service(Name = "IBCCustID")]
        public int CustId { get; set; }

        public string UserName { get; set; }

        [Service(Name = "FirstName")]
        public string FirstName { get; set; }

        [Service(Name = "LastName")]
        public string LastName { get; set; }

        [Service(Name = "IsClosed")]
        public bool IsClosed { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal YesterdayCash { get; set; }

        public decimal TotalCash { get; set; }

        public decimal Outstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal UsedCredit { get; set; }

        public bool IsHighlighted { get; set; }
    }
}