// ----------------------------------------------------------------------
// <copyright file="BetListCustomerInfo.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListCustomerInfo
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string IBCUsername { get; set; }

        public int CurrencyId { get; set; }
    }
}
