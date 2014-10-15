namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// Total bet service.
    /// </summary>
    public interface IBasketballService
    {
        IList<TotalBetsEntity> GetBasketballData(BasketballCriteria criteria);
    }
}