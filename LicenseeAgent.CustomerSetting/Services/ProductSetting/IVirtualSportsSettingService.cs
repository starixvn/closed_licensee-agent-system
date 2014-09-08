namespace LicenseeAgent.CustomerSetting.Services.ProductSetting
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IVirtualSportsSettingService : ISettingService
    {
        IList<UpdateResult> UpdateCustomer(UpdateVirtualSportsCustomerViewModel model);
    }
}
