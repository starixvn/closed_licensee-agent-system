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

        public bool LiveIndicator { get; set; }

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

        public string HomeFulltimeHandicap { get; set; }

        public string AwayFulltimeHandicap { get; set; }

        public string HomeFulltimeOverUnder { get; set; }

        public string AwayFulltimeOverUnder { get; set; }

        public string HomeFulltimeOneXTwo { get; set; }

        public string AwayFulltimeOneXTwo { get; set; }

        public string DrawFulltimeOneXTwo { get; set; }

        public string HomeFirstHalfHandicap { get; set; }
        
        public string HomeFullTimeOddEven { get; set; }

        public string AwayFullTimeOddEven { get; set; }

        public string HomeFirstHaflHandicap { get; set; }

        public string AwayFirstHalfHandicap { get; set; }

        public string HomeFirstHalfOverUnder { get; set; }

        public string AwayFirstHalfOverUnder { get; set; }

        public string HomeFirstHalfOneXTwo { get; set; }

        public string AwayFirstHalfOneXTwo { get; set; }

        public string DrawFirstHalfOneXTwo { get; set; }

        public string HomeFirstHaflOddEven { get; set; }

        public string AwayFirstHaflOddEven { get; set; }
    }
}
