namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// Outright service.
    /// </summary>
    public interface IOutrightService
    {
        IList<SportEntity> GetOutrightSports(OutrightSportCriteria criteria);

        IList<LeagueEntity> GetOutrightLeagues(OutrightLeagueCriteria criteria);

        IList<TeamEntity> GetOutrightTeam(OutrightTeamCriteria criteria);

        IList<OutrightEntity> GetOutrightData(OutrightCriteria criteria);
    }
}