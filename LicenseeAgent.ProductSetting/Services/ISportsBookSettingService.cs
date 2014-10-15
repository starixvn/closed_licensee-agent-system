namespace LicenseeAgent.ProductSetting.Services.ProductSetting
{
    using System.Collections.Generic;
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.ViewModels;

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
