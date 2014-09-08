namespace LicenseeAgent.CustomerSetting.Services.ProductSetting
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// ISettingRepository interface.
    /// </summary>
    public interface ISettingService
    {
        Product ProductId { get; }

        string Name { get; }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Verified.")]
        CustomerSetting2 GetSettingForInsert(SettingContext settingContext, out int errCode);

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Verified.")]
        CustomerSetting2 GetSettingForUpdate(SettingContext settingContext, out int errCode);
    }
}