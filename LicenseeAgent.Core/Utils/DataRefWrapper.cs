// --------------------------------------------------------------------------
// <copyright file="DataRefWrapper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utils
{
    using LicenseeAgent.Core.Enumerations;
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
            if (sportType == (int)SportType.OtherSports)
            {
                return betType == (int)BetType.MixParlay ? Las.MixSportsParlay : Las.OtherSports;
            }
            else
            {
                return RefData.GetSportName(sportType.ToString(), LanguageManager.TwoLetterISOLanguageName);
            }
        }

        public static string GetLeagueName(int leagueId)
        {
            return RefData.GetLeagueName(leagueId.ToString(), LanguageManager.TwoLetterISOLanguageName);
        }

        public static string GetTeamname(int teamId)
        {
            return RefData.GetTeamName(teamId.ToString(), LanguageManager.TwoLetterISOLanguageName);
        }
    }
}