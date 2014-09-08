namespace LicenseeAgent.CustomerSetting.Services.ProductSetting
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISportsBookSettingService : ISettingService
    {
        InsertResult InsertCustomer(InsertCustomerViewModel model);

        IList<UpdateResult> UpdateCustomer(UpdateSportsBookCustomerViewModel model);

        IList<UpdateResult> UpdateIndirectCustomer(UpdateSportsBookCustomerViewModel model);
    }
}
