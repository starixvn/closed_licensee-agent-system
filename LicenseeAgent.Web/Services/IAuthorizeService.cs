namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Web.Criteria;

    public interface IAuthorizeService
    {
        void CheckCurrencyPermission(CheckCurrencyPermissionCriteria criteria);
    }
}