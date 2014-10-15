namespace LicenseeAgent.Core.Configurations
{
    using System.Drawing;

    /// <summary>
    /// Constants class.
    /// </summary>
    public static class Constants
    {
        public const string AppSessionCaptcha = "LasCaptcha";
        public const string AppCookieLanguage = "LasLanguage";
        public const string AppCookieTopMenu = "LasTopMenu";
        public const string LocalTestCaptcha = "999999";

        public const int LrfAuthMinuteTimeout = 5;
        public const int MinimumUsernameLength = 6;
        public const int MaximumUsernameLength = 50;
        public const int SmallStringLength = 15;
        public const int MediumStringLength = 20;
        public const int LargeStringLength = 35;
        public const string EmptyString = "---";
        public const int PageSize = 20;
        public const int MemberLevel = -1;

        public const int CaptchaWidth = 119;
        public const int CaptchaHeight = 32;
        public static readonly Color CaptchaBackColor = Color.FromArgb(240, 238, 240);
        public static readonly Color CaptchaForeColor = Color.Black;
    }
}