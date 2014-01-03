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
        public int SportType { get; set; }

        public string SportName
        {
            get
            {
                return DataRefWrapper.GetSportName(SportType);
            }
        }

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return DataRefWrapper.GetLeagueName(LeagueId);
            }
        }

        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                return DataRefWrapper.GetTeamname(TeamId);
            }
        }

        public int BetCount { get; set; }

        public decimal Balance { get; set; }

        public decimal Forecast { get; set; }
    }
}