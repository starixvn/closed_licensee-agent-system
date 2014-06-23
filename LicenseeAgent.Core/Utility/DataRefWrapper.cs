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
    /// DataRefWrapper class.
    /// </summary>
    public static class DataRefWrapper
    {
        private static string RefLanguageCode
        {
            get
            {
                var languageCode = LanguageManager.Language;
                switch (languageCode)
                {
                    case "en-US": return "en";
                    case "zh-TW": return "ch"; // Traditional
                    case "zh-CN": return "cs"; // Simplified
                    case "ja-JP": return "jp";
                    case "th-TH": return "th"; // Thai

                    default: return "en";
                }
            }
        }

        public static string GetBetTypeName(int betType)
        {
            return RefData.GetBetTypeName(betType.ToString(), RefLanguageCode);
        }

        public static string GetBetTypeName(BetType betType)
        {
            return GetBetTypeName((int)betType);
        }

        public static string GetSportName(int sportType)
        {
            return GetSportName(sportType, 0);
        }

        public static string GetSportName(SportType sportType)
        {
            return GetSportName((int)sportType);
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

            return RefData.GetSportName(sportType.ToString(), RefLanguageCode);
        }

        public static string GetLeagueName(int leagueId)
        {
            return RefData.GetLeagueName(leagueId.ToString(), RefLanguageCode);
        }

        public static string GetTeamName(int teamId)
        {
            return RefData.GetTeamName(teamId.ToString(), RefLanguageCode);
        }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Returns product name.</returns>
        public static string GetProductName(int productId)
        {
            switch (productId)
            {
                case (int)Product.Product_0:
                    return Las.Sportsbook;

                case (int)Product.Product_2:
                    return Las.NumberGame;

                case (int)Product.Product_4:
                    return Las.LiveCasino;

                case (int)Product.Product_5:
                    return Las.VirtualSports;

                case (int)Product.Product_9:
                    return Las.PlaytechCasino;
            }

            return string.Format("Undefined product name {0}", productId);
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
                    switch (betTeam)
                    {
                        case "h":
                            return Las.HomeTeam;
                        case "a":
                            return Las.AwayTeam;
                    }

                    break;

                case (int)BetType.BetType_2:
                case (int)BetType.BetType_12:
                    switch (betTeam)
                    {
                        case "h":
                            return Las.Odd;
                        case "a":
                            return Las.Even;
                    }

                    break;

                case (int)BetType.BetType_3:
                case (int)BetType.BetType_8:
                    switch (betTeam)
                    {
                        case "h":
                            return Las.Over;
                        case "a":
                            return Las.Under;
                    }

                    break;

                case (int)BetType.BetType_4:
                    switch (betTeam)
                    {
                        case "0:5":
                            return "0-5 UP";
                        case "5:0":
                            return "5-0 UP";
                        default:
                            return betTeam.Replace(':', '-');
                    }

                case (int)BetType.BetType_5:
                case (int)BetType.BetType_15:
                case (int)BetType.BetType_28:
                    switch (betTeam)
                    {
                        case "1":
                            return Las.HomeTeam;
                        case "x":
                            return Las.Draw;
                        case "2":
                            return Las.AwayTeam;
                    }

                    break;

                case (int)BetType.BetType_6:
                    return betTeam == "7-over" ? Las.Score7Over : betTeam;

                case (int)BetType.BetType_13:
                    switch (betTeam)
                    {
                        case "hy":
                            return Las.HomeYes;
                        case "hn":
                            return Las.Even;
                        case "ay":
                            return Las.AwayYes;
                        case "an":
                            return Las.AwayNo;
                    }

                    break;

                case (int)BetType.BetType_127:
                case (int)BetType.BetType_14:
                    switch (betTeam)
                    {
                        case "1:1":
                            return "HF";
                        case "1:2":
                            return "HL";
                        case "2:1":
                            return "AF";
                        case "2:2":
                            return "AL";
                        case "0:0":
                            return "NG";
                    }

                    break;

                case (int)BetType.BetType_16:
                    switch (betTeam)
                    {
                        case "1:1":
                            return "HH";
                        case "1:0":
                            return "HD";
                        case "1:2":
                            return "HA";
                        case "2:1":
                            return "AH";
                        case "2:0":
                            return "AD";
                        case "2:2":
                            return "AA";
                        case "0:1":
                            return "DH";
                        case "0:0":
                            return "DD";
                        case "0:2":
                            return "DA";
                    }

                    break;

                case (int)BetType.BetType_24:
                    switch (betTeam)
                    {
                        case "1x":
                            return Las.HomeOrDraw;
                        case "12":
                            return Las.HomeOrAway;
                        case "2x":
                            return Las.AwayOrDraw;
                    }

                    break;

                case (int)BetType.BetType_26:
                    switch (betTeam)
                    {
                        case "b":
                            return Las.Both;
                        case "o":
                            return Las.One;
                        case "n":
                            return Las.NoGoal;
                    }

                    break;

                case (int)BetType.BetType_30:
                    switch (betTeam)
                    {
                        case "0:4":
                            return "0-4 UP";
                        case "4:0":
                            return "4-0 UP";
                        default:
                            return betTeam.Replace(':', '-');
                    }

                case (int)BetType.BetType_121:
                    switch (betTeam)
                    {
                        case "a":
                            return Las.AwayTeam;
                        case "x":
                            return Las.Draw;
                    }

                    break;

                case (int)BetType.BetType_122:
                    switch (betTeam)
                    {
                        case "h":
                            return Las.HomeTeam;
                        case "x":
                            return Las.Draw;
                    }

                    break;

                case (int)BetType.BetType_123:
                    switch (betTeam)
                    {
                        case "h":
                            return Las.Draw;
                        case "a":
                            return Las.NoDraw;
                    }

                    break;

                case (int)BetType.BetType_126:
                    return betTeam == "4-over" ? Las.Score4Over : betTeam;
            }

            throw new InvalidOperationException(string.Format("Undefined bet team name ({0}, {1})", betType, betTeam));
        }
    }
}