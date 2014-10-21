namespace LicenseeAgent.ProductSetting.Services.ProductSetting
{
    using System.Collections.Generic;
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IVirtualSportsSettingService : ISettingService
    {
        IList<UpdateResult> UpdateCustomer(UpdateCustomerViewModel model);
    }
}
