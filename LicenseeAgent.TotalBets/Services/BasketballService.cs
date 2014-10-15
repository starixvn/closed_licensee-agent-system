namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TotalBetsService class.
    /// </summary>
    public class BasketballService : QueryService, IBasketballService
    {
        public IList<TotalBetsEntity> GetBasketballData(BasketballCriteria criteria)
        {
            return GetEntites<TotalBetsEntity>(criteria);
        }
    }
}