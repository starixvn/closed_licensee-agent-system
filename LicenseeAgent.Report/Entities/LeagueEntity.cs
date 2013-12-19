// --------------------------------------------------------------------------
// <copyright file="LeagueEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Entities
{
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// League Entity.
    /// </summary>
    public class LeagueEntity
    {
        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return DataRefWrapper.GetLeagueName(LeagueId);
            }
        }
    }
}