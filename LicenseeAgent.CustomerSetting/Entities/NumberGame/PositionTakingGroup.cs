// ----------------------------------------------------------------------
// <copyright file="PositionTakingGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities.NumberGame
{
    using System.Collections.Generic;

    /// <summary>
    /// NumberGamePositionTakingGroup Class.
    /// </summary>
    public class PositionTakingGroup
    {
        public PositionTakingGroup()
        {
            PositionTakings = new List<PositionTaking>();
        }

        public int SportId { get; set; }

        public bool IsLive { get; set; }

        public IList<PositionTaking> PositionTakings { get; set; }
    }
}
