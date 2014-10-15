namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// Soccer service.
    /// </summary>
    public interface ISoccerService
    {
        IList<TotalBetsEntity> GetSoccerData(SoccerCriteria criteria);

        IList<SoccerDetailEntity> GetSoccerDetailData(SoccerDetailCriteria criteria);
    }
}