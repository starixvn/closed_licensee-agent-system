namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TotalBetsService class.
    /// </summary>
    public class SoccerService : QueryService, ISoccerService
    {
        public IList<TotalBetsEntity> GetSoccerData(SoccerCriteria criteria)
        {
            return GetEntites<TotalBetsEntity>(criteria);
        }

        public IList<SoccerDetailEntity> GetSoccerDetailData(SoccerDetailCriteria criteria)
        {
            return GetEntites<SoccerDetailEntity>(criteria);
        }
    }
}