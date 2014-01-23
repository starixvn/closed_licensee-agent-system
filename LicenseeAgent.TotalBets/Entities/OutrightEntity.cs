// -----------------------------------------------------------------------
// <copyright file="OutrightEntity.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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
                if (_sportName == null)
                {
                    _sportName = DataRefWrapper.GetSportName(SportType);
                }

                return _sportName;
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

        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                if (_teamName == null)
                {
                    _teamName = DataRefWrapper.GetTeamname(TeamId);
                }

                return _teamName;
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