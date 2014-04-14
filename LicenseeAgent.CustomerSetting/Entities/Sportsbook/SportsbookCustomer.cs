// --------------------------------------------------------------------------
// <copyright file="SportsbookCustomer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.29">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Entities.Sportsbook
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Entities;

    public class SportsbookCustomer
    {
        public Result Result { get; set; }

        public CustomerInfo CustInfo { get; set; }

        public CommissionSetting CommissionSetting { get; set; }

        public IList<BettingLimit> BettingLimits { get; set; }

        public IList<PositionTakingGroup> PositionTakingSetting { get; set; }
    }
}