// ----------------------------------------------------------------------
// <copyright file="BetListCustomerInfo.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Entities
{
    /// <summary>
    /// BetListCustomerInfo class.
    /// </summary>
    public class BetListCustomerInfo
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string IBCUsername { get; set; }

        public int CurrencyId { get; set; }
    }
}
