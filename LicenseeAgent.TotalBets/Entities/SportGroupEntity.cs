// -----------------------------------------------------------------------
// <copyright file="SportGroupEntity.cs" company="Nexcel Solutions Vietnam">
// LeagueGroupEntity.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Utility;

    public class SportGroupEntity
    {
        private string _sportName;

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

        public IList<LeagueGroupEntity> LeagueList { get; set; }
    }
}