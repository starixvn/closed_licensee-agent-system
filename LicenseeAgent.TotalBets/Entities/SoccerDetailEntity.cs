// -----------------------------------------------------------------------
// <copyright file="SoccerDetailEntity.cs" company="Nexcel Solutions Vietnam">
// SoccerDetailEntity.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// Soccer Detail Entity.
    /// </summary>
    public class SoccerDetailEntity
    {
        public int BetType { get; set; }

        public string BetTeam { get; set; }

        public int TotalTrans { get; set; }

        public decimal Stake { get; set; }

        public string BetTypeName { get; set; }

        public string BetTeamDescription { get; set; }

        public string TotalBetsText
        {
            get
            {
                return "(" + Formatter.FormatInt(TotalTrans) + ") " + (Stake % 1 == 0 ? Formatter.FormatInt(Stake) : Formatter.FormatDecimal(Stake));
            }
        }
    }
}