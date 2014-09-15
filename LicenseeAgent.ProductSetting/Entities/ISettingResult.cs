namespace LicenseeAgent.ProductSetting.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISettingResult
    {
        int ErrCode { get; set; }

        decimal MinPT { get; set; }

        decimal MaxPT { get; set; }

        int BetType { get; set; }

        int SportType { get; set; }

        decimal BetLimit { get; set; }

        bool IsLive { get; set; }

        decimal MaxCredit { get; set; }
    }
}