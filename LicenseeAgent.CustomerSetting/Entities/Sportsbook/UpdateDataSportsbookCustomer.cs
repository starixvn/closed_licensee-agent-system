// --------------------------------------------------------------------------
// <copyright file="UpdateDataSportsbookCustomer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.11.4">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Entities.Sportsbook
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateDataSportsbookCustomer
    {
        public CustomerInfo CustInfo { get; set; }

        public CommissionSetting CommissionSetting { get; set; }

        public IList<BettingLimit> BettingLimits { get; set; }

        public IList<PositionTaking> PositionTakingSetting { get; set; }
    }
}