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

    /// <summary>
    /// Match Entity.
    /// </summary>
    public class MatchEntity
    {
        public bool IsLive { get; set; }

        public int Index { get; set; }

        public string Time { get; set; }

        public string Event { get; set; }

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
