// --------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Configurations
{
    using System.Drawing;

    public static class Constants
    {
        public const string AppSessionCaptcha = "LasCaptcha";
        public const string AppCookieLanguage = "LasLanguage";
        public const string AppCookieTopMenu = "LasTopMenu";

        public const string BETLISTTICKETSTATUSRUNNING = "running";
        public const string BETLISTTICKETSTATUSWAITING = "waiting";

        public const int MinimumUsernameLength = 6;
        public const int MaximumUsernameLength = 50;
        public const int ShortStringLength = 15;
        public const int MediumStringLength = 20;
        public const int LargeStringLength = 35;
        public const string EmptyString = "---";
        public const int PageSize = 20;
        public const int MemberLevel = -1;

        public const int CaptchaWidth = 110;
        public const int CaptchaHeight = 32;
        public static readonly Color CaptchaBackColor = Color.FromArgb(240, 238, 240);
        public static readonly Color CaptchaForeColor = Color.Black;
    }
}