// -----------------------------------------------------------------------
// <copyright file="MatchEntity.cs" company="Nexcel Solutions Vietnam">
// MatchEntity.
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
    /// Match Entity.
    /// </summary>
    public class MatchEntity
    {
        public int MatchId { get; set; }

        public bool IsLive { get; set; }

        public int Index { get; set; }

        public string Time { get; set; }

        public int HomeId { get; set; }

        public string HomeName
        {
            get
            {
                return DataRefWrapper.GetTeamname(HomeId);
            }
        }

        public int AwayId { get; set; }

        public string AwayName
        {
            get
            {
                return DataRefWrapper.GetTeamname(AwayId);
            }
        }

        public string HomeFullTimeHandicap { get; set; }

        public string AwayFullTimeHandicap { get; set; }

        public string HomeFullTimeOverUnder { get; set; }

        public string AwayFullTimeOverUnder { get; set; }

        public string HomeFullTimeOneXTwo { get; set; }

        public string AwayFullTimeOneXTwo { get; set; }

        public string DrawFullTimeOneXTwo { get; set; }

        public string HomeFirstHaflHandicap { get; set; }

        public string AwayFirstHaflHandicap { get; set; }

        public string HomeFirstHaflOverUnder { get; set; }

        public string AwayFirstHaflOverUnder { get; set; }

        public string HomeFirstHaflOneXTwo { get; set; }

        public string AwayFirstHaflOneXTwo { get; set; }

        public string DrawFirstHaflOneXTwo { get; set; }
    }
}
