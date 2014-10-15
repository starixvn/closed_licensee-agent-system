namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TotalBetsService class.
    /// </summary>
    public class OutrightService : QueryService, IOutrightService
    {
        public IList<SportEntity> GetOutrightSports(OutrightSportCriteria criteria)
        {
            return GetEntites<SportEntity>(criteria);
        }

        public IList<LeagueEntity> GetOutrightLeagues(OutrightLeagueCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IList<TeamEntity> GetOutrightTeam(OutrightTeamCriteria criteria)
        {
            return GetEntites<TeamEntity>(criteria);
        }

        public IList<OutrightEntity> GetOutrightData(OutrightCriteria criteria)
        {
            return GetEntites<OutrightEntity>(criteria);
        }
    }
}