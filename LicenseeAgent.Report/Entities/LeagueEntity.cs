// -----------------------------------------------------------------------
// <copyright file="LeagueEntity.cs" company="Nexcel Solutions Vietnam">
// LeagueEntity.
// </copyright>
// -----------------------------------------------------------------------

using LicenseeAgent.Core.Utils;
namespace LicenseeAgent.Report.Entities
{
    /// <summary>
    /// LeagueEntity.
    /// </summary>
    public class LeagueEntity
    {
        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return DataRefWrapper.GetLeagueName(LeagueId);
            }
        }
    }
}