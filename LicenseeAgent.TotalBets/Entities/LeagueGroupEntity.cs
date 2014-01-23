// -----------------------------------------------------------------------
// <copyright file="LeagueGroupEntity.cs" company="Nexcel Solutions Vietnam">
// LeagueGroupEntity.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LeagueGroupEntity
    {
        private string _leagueName;

        public int LeagueId { get; set; }

        public bool LiveIndicator { get; set; }

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

        public IList<MatchEntity> MatchList { get; set; }
    }
}