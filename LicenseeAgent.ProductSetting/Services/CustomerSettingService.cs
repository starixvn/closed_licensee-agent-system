namespace LicenseeAgent.ProductSetting.Services
{
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.ProductSetting.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerSettingService : QueryService, ICustomerSettingService
    {
        /// <summary>
        /// Check user name.
        /// </summary>
        /// <param name="criteria">Check exist username criteria.</param>
        public void CheckExistUserName(CheckExistUserNameCriteria criteria)
        {
            Execute(criteria);
        }
    }
}