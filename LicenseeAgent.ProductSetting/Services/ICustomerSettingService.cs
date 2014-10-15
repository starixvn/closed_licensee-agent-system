namespace LicenseeAgent.ProductSetting.Services
{
    using LicenseeAgent.ProductSetting.Criteria;

    /// <summary>
    /// ICustomerSettingService interface.
    /// </summary>
    public interface ICustomerSettingService
    {
        void CheckExistUserName(CheckExistUserNameCriteria criteria);
    }
}