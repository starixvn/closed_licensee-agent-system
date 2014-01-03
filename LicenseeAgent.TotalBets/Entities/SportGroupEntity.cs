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
        public int SportType { get; set; }

        public string SportName
        {
            get
            {
                return DataRefWrapper.GetSportName(SportType);
            }
        }

        public IList<LeagueGroupEntity> LeagueList { get; set; }
    }
}
