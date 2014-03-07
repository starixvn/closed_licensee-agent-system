// -----------------------------------------------------------------------
// <copyright file="TotalBetsUtility.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Utility
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TotalBetsUtility
    {
        /// <summary>
        /// Get main display data to view.
        /// </summary>
        /// <param name="dataList">Data list value.</param>
        /// <returns>Group league list view data.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public static IEnumerable<LeagueGroupEntity> MapTotalBetsMainData(IEnumerable<TotalBetsEntity> dataList)
        {
            var timeLiveTemplate = "{0}-{1}<br /><span class='live'>" + Las.Live + "!</span>";
            var result = new List<LeagueGroupEntity>();
            var leagueGroupList = dataList.Select(x => new { x.LeagueId, x.LiveIndicator }).Distinct().ToList();
            foreach (var league in leagueGroupList)
            {
                var matchList = new List<MatchEntity>();
                var matchsInLeague = dataList.Where(x => x.LeagueId == league.LeagueId && x.LiveIndicator == league.LiveIndicator).ToList();
                var matchGroupList = matchsInLeague.Select(x => new { x.MatchId, x.LiveHomeScore, x.LiveAwayScore, x.LiveIndicator }).Distinct().ToList();
                foreach (var matchGroup in matchGroupList)
                {
                    var matches = matchsInLeague.Where(x =>
                        x.MatchId == matchGroup.MatchId && x.LiveHomeScore == matchGroup.LiveHomeScore &&
                        x.LiveAwayScore == matchGroup.LiveAwayScore && x.LiveIndicator == matchGroup.LiveIndicator).ToList();
                    matchList.Add(new MatchEntity
                    {
                        MatchId = matches.FirstOrDefault().MatchId,
                        LiveIndicator = matchGroup.LiveIndicator,
                        Time = matchGroup.LiveIndicator 
                            ? string.Format(timeLiveTemplate, matchGroup.LiveHomeScore, matchGroup.LiveAwayScore)
                            : matches.FirstOrDefault().ShowTime,
                        LiveHomeScore = matchGroup.LiveHomeScore,
                        LiveAwayScore = matchGroup.LiveAwayScore,
                        HomeId = matches.FirstOrDefault().HomeId,
                        AwayId = matches.FirstOrDefault().AwayId,
                        IsNeutral = matches.FirstOrDefault().IsNeutral,
                        HomeFulltimeHandicap = SetTotalTransAndStakeValues(matches, BetType.BetType_1, "h"),
                        AwayFulltimeHandicap = SetTotalTransAndStakeValues(matches, BetType.BetType_1, "a"),
                        HomeFulltimeOddEven = SetTotalTransAndStakeValues(matches, BetType.BetType_2, "h"),
                        AwayFulltimeOddEven = SetTotalTransAndStakeValues(matches, BetType.BetType_2, "a"),
                        HomeFulltimeOverUnder = SetTotalTransAndStakeValues(matches, BetType.BetType_3, "h"),
                        AwayFulltimeOverUnder = SetTotalTransAndStakeValues(matches, BetType.BetType_3, "a"),
                        HomeFulltimeOneXTwo = SetTotalTransAndStakeValues(matches, BetType.BetType_5, "1"),
                        AwayFulltimeOneXTwo = SetTotalTransAndStakeValues(matches, BetType.BetType_5, "2"),
                        DrawFulltimeOneXTwo = SetTotalTransAndStakeValues(matches, BetType.BetType_5, "x"),
                        HomeFirstHalfHandicap = SetTotalTransAndStakeValues(matches, BetType.BetType_7, "h"),
                        AwayFirstHalfHandicap = SetTotalTransAndStakeValues(matches, BetType.BetType_7, "a"),
                        HomeFirstHalfOverUnder = SetTotalTransAndStakeValues(matches, BetType.BetType_8, "h"),
                        AwayFirstHalfOverUnder = SetTotalTransAndStakeValues(matches, BetType.BetType_8, "a"),
                        HomeFirstHalfOddEven = SetTotalTransAndStakeValues(matches, BetType.BetType_12, "h"),
                        AwayFirstHalfOddEven = SetTotalTransAndStakeValues(matches, BetType.BetType_12, "a"),
                        HomeFirstHalfOneXTwo = SetTotalTransAndStakeValues(matches, BetType.BetType_15, "1"),
                        AwayFirstHalfOneXTwo = SetTotalTransAndStakeValues(matches, BetType.BetType_15, "2"),
                        DrawFirstHalfOneXTwo = SetTotalTransAndStakeValues(matches, BetType.BetType_15, "x"),
                        HomeFulltimeMoneyLine = SetTotalTransAndStakeValues(matches, BetType.BetType_20, "h"),
                        AwayFulltimeMoneyLine = SetTotalTransAndStakeValues(matches, BetType.BetType_20, "a")
                    });
                }

                var orderedMatchList = matchList                        
                        .OrderByDescending(x => x.LiveHomeScore)
                        .ThenByDescending(a => a.LiveAwayScore);
                yield return new LeagueGroupEntity
                {
                    LeagueId = league.LeagueId,
                    LiveIndicator = league.LiveIndicator,
                    MatchList = orderedMatchList.OrderBy(h => h.HomeName).ToList()
                };
            }
        }

        /// <summary>
        /// Set total transaction and stake value.
        /// </summary>
        /// <param name="matchs">Match list.</param>
        /// <param name="enumBetType">Bet type value.</param>
        /// <param name="betTeam">Bet team value.</param>
        /// <returns>Total bet string.</returns>
        private static string SetTotalTransAndStakeValues(IEnumerable<TotalBetsEntity> matchs, BetType enumBetType, string betTeam)
        {
            var betType = (int)enumBetType;
            var matchByBet = matchs.Where(x => x.BetType == betType && x.BetTeam == betTeam);
            if (matchByBet.Any())
            {
                var match = matchByBet.FirstOrDefault();
                var template = "({0}) {1}";
                var betCountString = Formatter.FormatInt(match.TotalTrans);
                var stakeString = match.Stake % 1 == 0 ? Formatter.FormatInt(match.Stake) : Formatter.FormatDecimal(match.Stake);

                return string.Format(template, betCountString, stakeString);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
