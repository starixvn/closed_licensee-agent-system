namespace LicenseeAgent.CustomerSetting.Services
{
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// ISettingRepository interface.
    /// </summary>
    public interface ISettingService
    {
        Product ProductId { get; }

        string Name { get; }

        CustomerSetting2 GetSettingForInsert(SettingContext settingContext, out int errCode);

        CustomerSetting2 GetSettingForUpdate(SettingContext settingContext, out int errCode);
    }
}