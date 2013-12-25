// -----------------------------------------------------------------------
// <copyright file="TotalBetsService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TotalBetsService : QueryService, ITotalBetsService
    {
        public IEnumerable<LeagueEntity> GetLeagues(SoccerLeagueCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IEnumerable<EventEntity> GetEventsByLeague(SoccerEventCriteria criteria)
        {
            return GetEntites<EventEntity>(criteria);
        }

        public IEnumerable<SoccerDataEntity> GetSoccerData(SoccerCriteria criteria)
        {
            return GetEntites<SoccerDataEntity>(criteria);
        }

        public IEnumerable<SoccerDetailEntity> GetSoccerDetailData(SoccerDetailCriteria criteria)
        {
            return GetEntites<SoccerDetailEntity>(criteria);
        }
        
        public IEnumerable<TotalBetsEntity> GetBasketballData(BasketballCriteria criteria)
        {
            return GetEntites<TotalBetsEntity>(criteria);
        }
    }
}