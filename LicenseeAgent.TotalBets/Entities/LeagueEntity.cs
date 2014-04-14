// --------------------------------------------------------------------------
// <copyright file="LeagueEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// League Entity.
    /// </summary>
    public class LeagueEntity
    {
        private string _leagueName;

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return _leagueName ?? (_leagueName = DataRefWrapper.GetLeagueName(LeagueId));
            }

            set
            {
                _leagueName = value;
            }
        }
    }
}