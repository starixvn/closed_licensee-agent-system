namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Criteria;

    /// <summary>
    /// AuthorizeService class.
    /// </summary>
    public class AuthorizeService : QueryService, IAuthorizeService
    {
        public void CheckCurrencyPermission(CheckCurrencyPermissionCriteria criteria)
        {
            Execute(criteria);
        }
    }
}