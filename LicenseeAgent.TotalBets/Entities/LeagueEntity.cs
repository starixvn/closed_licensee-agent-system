// --------------------------------------------------------------------------
// <copyright file="LeagueEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utils;

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
                if (_leagueName == null)
                {
                    _leagueName = DataRefWrapper.GetLeagueName(LeagueId);
                }

                return _leagueName;
            }

            set
            {
                _leagueName = value;
            }
        }
    }
}