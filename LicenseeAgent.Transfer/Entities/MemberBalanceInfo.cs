// --------------------------------------------------------------------------
// <copyright file="MemberBalanceInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Entities
{
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Attributes;

    public class MemberBalanceInfo
    {
        public long RowNo { get; set; }

        [Service(Name = "IBCCustID")]
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Status { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal UsedCredit { get; set; }

        public bool IsHighlighted { get; set; }

        public string CustomerName
        {
            get
            {
                return Extensions.FormatCustomerName(FirstName, LastName);
            }
        }
    }
}