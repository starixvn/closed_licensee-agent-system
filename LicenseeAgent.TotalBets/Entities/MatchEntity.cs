// -----------------------------------------------------------------------
// <copyright file="MatchEntity.cs" company="Nexcel Solutions Vietnam">
// MatchEntity.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// Match Entity.
    /// </summary>
    public class MatchEntity
    {
        private string _homeName;

        private string _awayName;

        public int MatchId { get; set; }

        public bool LiveIndicator { get; set; }

        public string Time { get; set; }

        public int HomeId { get; set; }

        public bool IsNeutral { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }

        public string HomeName
        {
            get
            {
                if (_homeName == null)
                {
                    var neutral = IsNeutral ? " (N)" : string.Empty;
                    _homeName = DataRefWrapper.GetTeamname(HomeId) + neutral;
                }

                return _homeName;
            }
            set
            {
                _homeName = value;
            }
        }

        public int AwayId { get; set; }

        public string AwayName
        {
            get
            {
                if (_awayName == null)
                {
                    _awayName = DataRefWrapper.GetTeamname(AwayId);
                }
                return _awayName;
            }
            set
            {
                _awayName = value;
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

        public string HomeFulltimeOddEven { get; set; }

        public string AwayFulltimeOddEven { get; set; }

        public string HomeFulltimeMoneyLine { get; set; }

        public string AwayFulltimeMoneyLine { get; set; }

        public string HomeFirstHaflHandicap { get; set; }

        public string AwayFirstHalfHandicap { get; set; }

        public string HomeFirstHalfOverUnder { get; set; }

        public string AwayFirstHalfOverUnder { get; set; }

        public string HomeFirstHalfOneXTwo { get; set; }

        public string AwayFirstHalfOneXTwo { get; set; }

        public string DrawFirstHalfOneXTwo { get; set; }

        public string HomeFirstHalfOddEven { get; set; }

        public string AwayFirstHalfOddEven { get; set; }
    }
}