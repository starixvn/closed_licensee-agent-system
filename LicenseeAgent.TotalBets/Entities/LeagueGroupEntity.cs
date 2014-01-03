// -----------------------------------------------------------------------
// <copyright file="LeagueGroupEntity.cs" company="Nexcel Solutions Vietnam">
// LeagueGroupEntity.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LeagueGroupEntity
    {
        public int LeagueId { get; set; }

        public bool LiveIndicator { get; set; }

        public string LeagueName
        {
            get
            {
                return DataRefWrapper.GetLeagueName(LeagueId);
            }
        }

        public IList<MatchEntity> MatchList { get; set; }
    }
}
