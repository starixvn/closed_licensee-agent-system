// --------------------------------------------------------------------------
// <copyright file="PositionTaking.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.17">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Entities.Sportsbook
{
    public class PositionTaking
    {
        public int SportId { get; set; }

        public int BetTypeId { get; set; }

        public bool IsLive { get; set; }

        public decimal Min { get; set; }

        public decimal Max { get; set; }

        public decimal Preset { get; set; }

        public decimal Value { get; set; }

        public bool ApplyAll { get; set; }

        public PositionTaking Upline { get; set; }

        public PositionTaking AllDownLine { get; set; }
    }
}