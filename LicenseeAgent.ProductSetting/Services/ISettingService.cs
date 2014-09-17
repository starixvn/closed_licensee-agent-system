namespace LicenseeAgent.ProductSetting.Services.ProductSetting
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.ProductSetting.Entities;

    /// <summary>
    /// ISettingRepository interface.
    /// </summary>
    public interface ISettingService
    {
        ProductCode ProductId { get; }

        string Name { get; }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Verified.")]
        CustomerSetting2 GetSettingForInsert(SettingContext settingContext, out int errCode);

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Verified.")]
        CustomerSetting2 GetSettingForUpdate(SettingContext settingContext, out int errCode);
    }
}