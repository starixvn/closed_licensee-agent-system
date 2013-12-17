// --------------------------------------------------------------------------
// <copyright file="DataRefWrapper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.11.7">Created</change>
// </history>

namespace LicenseeAgent.Core.Utils
{
    using LicenseeAgent.Resources;
    using RemotingRef;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DataRefWrapper
    {
        public static string GetBetTypeName(int betType)
        {
            return RefData.GetBetTypeName(betType.ToString(), LanguageManager.TwoLetterISOLanguageName);
        }

        /// <summary>
        /// Gets the name of the sport.
        /// </summary>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <returns>Return the sport name.</returns>
        public static string GetSportName(int sportType, int betType = 0)
        {
            if (sportType == 99)
            {
                return betType == 9 ? Las.MixSportsParlay : Las.OtherSports;
            }
            else
            {
                return RefData.GetSportName(sportType.ToString(), LanguageManager.TwoLetterISOLanguageName);
            }
        }
    }
}