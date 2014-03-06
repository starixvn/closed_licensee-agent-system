// --------------------------------------------------------------------------
// <copyright file="DataRefWrapper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System;
    using System.Diagnostics.CodeAnalysis;
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

        public static string GetSportName(int sportType)
        {
            return GetSportName(sportType, 0);
        }

        /// <summary>
        /// Gets the name of the sport.
        /// </summary>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <returns>Return the sport name.</returns>
        public static string GetSportName(int sportType, int betType)
        {
            if (sportType == (int)SportType.SportType_99)
            {
                return betType == (int)BetType.BetType_9 ? Las.MixSportsParlay : Las.OtherSports;
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

        /// <summary>
        /// Gets the name of the bet team.
        /// </summary>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="betTeam">The bet team.</param>
        /// <returns>Returns bet team title.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public static string GetBetTeamName(int betType, string betTeam)
        {
            switch (betType)
            {
                case (int)BetType.BetType_1:
                case (int)BetType.BetType_7:
                case (int)BetType.BetType_20:
                case (int)BetType.BetType_25:
                case (int)BetType.BetType_27:
                    if (betTeam == "h")
                    {
                        return Las.HomeTeam;
                    }
                    else if (betTeam == "a")
                    {
                        return Las.AwayTeam;
                    }

                    break;

                case (int)BetType.BetType_2:
                case (int)BetType.BetType_12:
                    if (betTeam == "h")
                    {
                        return Las.Odd;
                    }
                    else if (betTeam == "a")
                    {
                        return Las.Even;
                    }

                    break;

                case (int)BetType.BetType_3:
                case (int)BetType.BetType_8:
                    if (betTeam == "h")
                    {
                        return Las.Over;
                    }
                    else if (betTeam == "a")
                    {
                        return Las.Under;
                    }

                    break;

                case (int)BetType.BetType_4:
                    if (betTeam == "0:5")
                    {
                        return "0-5 UP";
                    }
                    else if (betTeam == "5:0")
                    {
                        return "5-0 UP";
                    }
                    else
                    {
                        return betTeam.Replace(':', '-');
                    }

                case (int)BetType.BetType_5:
                case (int)BetType.BetType_15:
                case (int)BetType.BetType_28:
                    if (betTeam == "1")
                    {
                        return Las.HomeTeam;
                    }
                    else if (betTeam == "x")
                    {
                        return Las.Draw;
                    }
                    else if (betTeam == "2")
                    {
                        return Las.AwayTeam;
                    }

                    break;

                case (int)BetType.BetType_6:
                    if (betTeam == "7-over")
                    {
                        return Las.Score7Over;
                    }
                    else
                    {
                        return betTeam;
                    }

                case (int)BetType.BetType_13:
                    if (betTeam == "hy")
                    {
                        return Las.HomeYes;
                    }
                    else if (betTeam == "hn")
                    {
                        return Las.Even;
                    }
                    else if (betTeam == "ay")
                    {
                        return Las.AwayYes;
                    }
                    else if (betTeam == "an")
                    {
                        return Las.AwayNo;
                    }

                    break;

                case (int)BetType.BetType_14:
                    if (betTeam == "1:1")
                    {
                        return "HF";
                    }
                    else if (betTeam == "1:2")
                    {
                        return "HL";
                    }
                    else if (betTeam == "2:1")
                    {
                        return "AF";
                    }
                    else if (betTeam == "2:2")
                    {
                        return "AL";
                    }
                    else if (betTeam == "0:0")
                    {
                        return "NG";
                    }

                    break;

                case (int)BetType.BetType_16:
                    if (betTeam == "1:1")
                    {
                        return "HH";
                    }
                    else if (betTeam == "1:0")
                    {
                        return "HD";
                    }
                    else if (betTeam == "1:2")
                    {
                        return "HA";
                    }
                    else if (betTeam == "2:1")
                    {
                        return "AH";
                    }
                    else if (betTeam == "2:0")
                    {
                        return "AD";
                    }
                    else if (betTeam == "2:2")
                    {
                        return "AA";
                    }
                    else if (betTeam == "0:1")
                    {
                        return "DH";
                    }
                    else if (betTeam == "0:0")
                    {
                        return "DD";
                    }
                    else if (betTeam == "0:2")
                    {
                        return "DA";
                    }

                    break;

                case (int)BetType.BetType_24:
                    if (betTeam == "1x")
                    {
                        return Las.HomeOrDraw;
                    }
                    else if (betTeam == "12")
                    {
                        return Las.HomeOrAway;
                    }
                    else if (betTeam == "2x")
                    {
                        return Las.AwayOrDraw;
                    }

                    break;

                case (int)BetType.BetType_26:
                    if (betTeam == "b")
                    {
                        return Las.Both;
                    }
                    else if (betTeam == "o")
                    {
                        return Las.One;
                    }
                    else if (betTeam == "n")
                    {
                        return Las.NoGoal;
                    }

                    break;

                case (int)BetType.BetType_30:
                    if (betTeam == "0:4")
                    {
                        return "0-4 UP";
                    }
                    else if (betTeam == "4:0")
                    {
                        return "4-0 UP";
                    }
                    else if (betTeam == "2")
                    {
                        return betTeam.Replace(':', '-');
                    }

                    break;

                case (int)BetType.BetType_121:
                    if (betTeam == "a")
                    {
                        return Las.AwayTeam;
                    }
                    else if (betTeam == "x")
                    {
                        return Las.Draw;
                    }

                    break;

                case (int)BetType.BetType_122:
                    if (betTeam == "h")
                    {
                        return Las.HomeTeam;
                    }
                    else if (betTeam == "x")
                    {
                        return Las.Draw;
                    }

                    break;

                case (int)BetType.BetType_123:
                    if (betTeam == "h")
                    {
                        return Las.Draw;
                    }
                    else if (betTeam == "a")
                    {
                        return Las.NoDraw;
                    }

                    break;
            }

            throw new InvalidOperationException(string.Format("Undefined bet team name ({0}, {1})", betType, betTeam));
        }
    }
}