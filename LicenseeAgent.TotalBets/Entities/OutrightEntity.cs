// -----------------------------------------------------------------------
// <copyright file="OutrightEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utility;

    public class OutrightEntity
    {
        private string _sportName;

        private string _leagueName;

        private string _teamName;

        public int SportType { get; set; }

        public string SportName
        {
            get
            {
                return _sportName ?? (_sportName = DataRefWrapper.GetSportName(SportType));
            }

            set
            {
                _sportName = value;
            }
        }

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

        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                return _teamName ?? (_teamName = DataRefWrapper.GetTeamName(TeamId));
            }

            set
            {
                _teamName = value;
            }
        }

        public int BetCount { get; set; }

        public decimal Balance { get; set; }

        public decimal Forecast { get; set; }
    }
}