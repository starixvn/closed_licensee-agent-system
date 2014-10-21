namespace LicenseeAgent.ProductSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.Services.ProductSetting;
    using LicenseeAgent.ProductSetting.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IKenoSettingService : ISettingService
    {
        IList<UpdateResult> UpdateCustomer(UpdateCustomerViewModel model);
    }
}
