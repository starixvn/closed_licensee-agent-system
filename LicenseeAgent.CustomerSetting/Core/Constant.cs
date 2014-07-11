namespace LicenseeAgent.CustomerSetting.Core
{
    using System.Diagnostics.CodeAnalysis;
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// Constant information.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2100:CodeLineMustNotBeLongerThan",
            Justification = "Reviewed. Suppression is OK here.")]
    public static class Constant
    {
        public const string PTGridConfig = "~/App_Data/CustomerSetting/PTGrid.xml";
        public const string FormatRootXml = "<ROOT>{0}</ROOT>";
        public const string FormatAgentPositionTakingXml = "<PositionTaking SportType=\"{0}\" BetType=\"{1}\" IsLive=\"{2}\" MaxUpper=\"{3}\" MaxGiven=\"{4}\" MinGiven=\"{5}\"></PositionTaking>";
        public const string FormatAgentEditAgentPositionTakingXml = "<PositionTaking SportType=\"{0}\" BetType=\"{1}\" IsLive=\"{2}\" MaxUpper=\"{3}\" MaxGiven=\"{4}\" MinGiven=\"{5}\" ApplyAll=\"{6}\"></PositionTaking>";
        public const string FormatMemberPositionTakingXml = "<PositionTaking SportType=\"{0}\" BetType=\"{1}\" IsLive=\"{2}\" PT=\"{3}\" ></PositionTaking>";
        public const string FormatAgentCommissionSettingXml = "<Discount SportType=\"{0}\" BetType=\"{1}\" DiscountA=\"{2}\" DiscountB=\"{3}\" DiscountC=\"{4}\" DiscountD=\"{5}\"></Discount>";
        public const string FormatMemberCommissionSettingXml = "<Discount SportType=\"{0}\" BetType=\"{1}\" Discount=\"{2}\" ></Discount>";
        public const string FormatBettingLimitXml = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" Min=\"{2}\" Max=\"{3}\" MaxPerMatch=\"{4}\"></BetLimit>";
        public const string FormatBettingLimit3RDXml = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" Min=\"{2}\" Max=\"{3}\" MaxPerMatch=\"{4}\"  MaxPerBall=\"{5}\"></BetLimit>";
        public const int StepMulDiv = 100;
        public const decimal RoundingNumber = 0.0049m;
        public const int NumberDigist = 2;
        public const int PTRate = 10000;

        public static string DefaultLanguage
        {
            get
            {
                return ModelSettings.DefaultLanguage;
            }
        }
    }
}