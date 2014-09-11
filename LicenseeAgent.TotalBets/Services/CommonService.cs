namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TotalBetsService class.
    /// </summary>
    public class CommonService : QueryService, ICommonService
    {
        public IList<LeagueEntity> GetLeagues(LeagueCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IList<EventEntity> GetEventsByLeague(EventCriteria criteria)
        {
            return GetEntites<EventEntity>(criteria);
        }
    }
}