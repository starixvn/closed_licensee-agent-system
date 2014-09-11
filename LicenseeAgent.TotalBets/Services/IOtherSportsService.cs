namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// Total bet service.
    /// </summary>
    public interface IOtherSportsService
    {
        IList<TotalBetsEntity> GetOtherSportsData(OtherSportsCriteria criteria);

        IList<SportEntity> GetOtherSportsList(OtherSportsSportListCriteria criteria);

        IList<LeagueEntity> GetOtherSportsLeagues(OtherSportsLeagueListCriteria criteria);

        IList<EventEntity> GetOtherSportsEventsByLeague(OtherSportsEventListCriteria criteria);
    }
}