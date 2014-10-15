namespace LicenseeAgent.Home.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Home.Criteria.TopMenu;
    using LicenseeAgent.Home.Entities.TopMenu;

    /// <summary>
    /// ITopMenuService class.
    /// </summary>
    public interface ITopMenuService
    {
        IList<LicenseeEntity> GetLicensees(int userId);

        IList<CurrencyEntity> GetLicenseeCurrencies(GetCurrencyOfLicenseeCriteria criteria);
    }
}