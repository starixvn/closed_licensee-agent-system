// --------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Configurations
{
    using System.Drawing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Constants
    {
        public const string AppSessionCaptcha = "LasCaptcha";
        public const string AppSessionLanguage = "LasLanguage";

        public const string BETLISTTICKETSTATUSRUNNING = "running";
        public const string BETLISTTICKETSTATUSWAITING = "waiting";

        public const int CaptchaWidth = 110;
        public const int CaptchaHeight = 32;
        public static readonly Color CaptchaBackColor = Color.FromArgb(240, 238, 240);
        public static readonly Color CaptchaForeColor = Color.Black;
    }
}