namespace LicenseeAgent.CustomerSetting.Core
{
    using System.Diagnostics.CodeAnalysis;
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// Constant information.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2100:CodeLineMustNotBeLongerThan",
            Justification = "Reviewed. Suppression is OK here.")]
    public static class Constant
    {
        public const string PT_GRID_CONFIG = "~/App_Data/CustomerSetting/PTGrid.xml";
        public const string FORMAT_XML_ROOT = "<ROOT>{0}</ROOT>";
        public const string FORMAT_AGENT_POSITION_TAKING_XML = "<PositionTaking SportType=\"{0}\" BetType=\"{1}\" IsLive=\"{2}\" MaxUpper=\"{3}\" MaxGiven=\"{4}\" MinGiven=\"{5}\"></PositionTaking>";
        public const string FORMAT_AGENT_EDIT_AGENT_POSITION_TAKING_XML = "<PositionTaking SportType=\"{0}\" BetType=\"{1}\" IsLive=\"{2}\" MaxUpper=\"{3}\" MaxGiven=\"{4}\" MinGiven=\"{5}\" ApplyAll=\"{6}\"></PositionTaking>";
        public const string FORMAT_MEMBER_POSITION_TAKING_XML = "<PositionTaking SportType=\"{0}\" BetType=\"{1}\" IsLive=\"{2}\" PT=\"{3}\" ></PositionTaking>";
        public const string FORMAT_AGENT_COMMISSION_SETTING_XML = "<Discount SportType=\"{0}\" BetType=\"{1}\" DiscountA=\"{2}\" DiscountB=\"{3}\" DiscountC=\"{4}\" DiscountD=\"{5}\"></Discount>";
        public const string FORMAT_MEMBER_COMMISSION_SETTING_XML = "<Discount SportType=\"{0}\" BetType=\"{1}\" Discount=\"{2}\" ></Discount>";
        public const string FORMAT_BETTING_LIMIT_XML = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" Min=\"{2}\" Max=\"{3}\" MaxPerMatch=\"{4}\"></BetLimit>";
        public const string FORMAT_BETTING_LIMIT_XML_3RD = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" Min=\"{2}\" Max=\"{3}\" MaxPerMatch=\"{4}\"  MaxPerBall=\"{5}\"></BetLimit>";
        public const int STEP_MUL_DIV = 100;
        public const decimal ROUNDING_NUMBER = 0.0049m;
        public const int NUMBER_DIGIST = 2;
        public const int PT_RATE = 10000;

        public static string DefaultLanguage
        {
            get
            {
                return ModelSettings.DefaultLanguage;
            }
        }
    }
}