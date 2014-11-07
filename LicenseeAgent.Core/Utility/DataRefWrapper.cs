namespace LicenseeAgent.Core.Utility
{
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

        public static string GetKenoBetTypeName(int betType)
        {
            return RefData.GetKenoBettype(betType.ToString(), RefLanguageCode);
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
        /// Gets the ID of the product.
        /// </summary>
        /// <param name="sportTypeId">The sport identifier.</param>
        /// <returns>Returns product ID.</returns>
        public static int GetProductIdBySportType(int sportTypeId)
        {
            switch (sportTypeId)
            {
                case (int)SportType.SportType_162:
                    return (int)ProductCode.Product_4;

                case (int)SportType.SportType_202:
                    return (int)ProductCode.Product_8;

                case (int)SportType.SportType_161:
                    return (int)ProductCode.Product_2;

                case (int)SportType.SportType_180:
                case (int)SportType.SportType_181:
                case (int)SportType.SportType_182:
                case (int)SportType.SportType_183:
                case (int)SportType.SportType_184:
                case (int)SportType.SportType_185:
                case (int)SportType.SportType_186:
                    return (int)ProductCode.Product_5;

                default:
                    return (int)ProductCode.Product_0;
            }
        }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <param name="productCode">The product identifier.</param>
        /// <returns>Returns product name.</returns>
        public static string GetProductName(ProductCode productCode)
        {
            switch (productCode)
            {
                case ProductCode.Product_0:
                    return Las.Sportsbook;

                case ProductCode.Product_1:
                    return Las.Racing;

                case ProductCode.Product_2:
                    return Las.NumberGame;

                case ProductCode.Product_4:
                    return Las.LiveCasino;

                case ProductCode.Product_5:
                    return Las.VirtualSports;

                case ProductCode.Product_6:
                    return Las.RNGCasino;

                case ProductCode.Product_7:
                    return Las.Bingo;

                case ProductCode.Product_8:
                    return Las.Keno;

                case ProductCode.Product_9:
                    return Las.PlaytechCasino;

                case ProductCode.Product_1000:
                    return Las.HrFixedOdds;
            }

            return string.Format("Undefined product name {0}", productCode);
        }

        public static string GetProductName(int productId)
        {
            return GetProductName((ProductCode)productId);
        }

        /// <summary>
        /// Gets the name of the bet team.
        /// </summary>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="betTeam">The bet team.</param>
        /// <returns>Returns bet team title.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Review")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Fix later.")]
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

            return string.Format("Undefined bet team name ({0}, {1})", betType, betTeam);
        }

        /// <summary>
        /// Gets the name of the legend type.
        /// </summary>
        /// <param name="legendType">Type of the legend.</param>
        /// <returns>Returns name of legend model.</returns>
        public static string GetLegendTypeName(int legendType)
        {
            switch (legendType)
            {
                case (int)LegendType.Credit:
                    return Las.Credit;

                case (int)LegendType.Deposit:
                    return Las.Cash;
            }

            return string.Format("Undefined legend type ({0})", legendType);
        }
    }
}