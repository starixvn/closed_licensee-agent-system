// -----------------------------------------------------------------------
// <copyright file="SportGroupEntity.cs" company="Nexcel Solutions Vietnam">
// LeagueGroupEntity.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using System.Collections.Generic;
    using Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SportGroupEntity
    {
        private string _sportName;

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

        public IList<LeagueGroupEntity> LeagueList { get; set; }
    }
}
