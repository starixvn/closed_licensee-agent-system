namespace LicenseeAgent.Home.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Home.Criteria.TopMenu;
    using LicenseeAgent.Home.Entities.TopMenu;

    /// <summary>
    /// TopMenuService class.
    /// </summary>
    public class TopMenuService : QueryService, ITopMenuService
    {
        public IList<LicenseeEntity> GetLicensees(int userId)
        {
            var criteria = new GetLicenseeListCriteria
            {
                UserId = userId
            };

            return GetEntites<LicenseeEntity>(criteria);
        }

        public IList<CurrencyEntity> GetLicenseeCurrencies(GetCurrencyOfLicenseeCriteria criteria)
        {
            return GetEntites<CurrencyEntity>(criteria);
        }
    }
}