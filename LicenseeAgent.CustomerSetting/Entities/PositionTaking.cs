// --------------------------------------------------------------------------
// <copyright file="PositionTaking.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    /// <summary>
    /// PositionTaking class.
    /// </summary>
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