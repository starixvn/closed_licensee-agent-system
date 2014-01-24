// -----------------------------------------------------------------------
// <copyright file="TotalBetsUtility.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Resources;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TotalBetsUtility
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
                        HomeFulltimeHandicap = SetTotalTransAndStakeValues(matches, BetType.FulltimeHandicap, "h"),
                        AwayFulltimeHandicap = SetTotalTransAndStakeValues(matches, BetType.FulltimeHandicap, "a"),
                        HomeFulltimeOddEven = SetTotalTransAndStakeValues(matches, BetType.FulltimeOddEven, "h"),
                        AwayFulltimeOddEven = SetTotalTransAndStakeValues(matches, BetType.FulltimeOddEven, "a"),
                        HomeFulltimeOverUnder = SetTotalTransAndStakeValues(matches, BetType.FulltimeOverUnder, "h"),
                        AwayFulltimeOverUnder = SetTotalTransAndStakeValues(matches, BetType.FulltimeOverUnder, "a"),
                        HomeFulltimeOneXTwo = SetTotalTransAndStakeValues(matches, BetType.FulltimeOneXTwo, "1"),
                        AwayFulltimeOneXTwo = SetTotalTransAndStakeValues(matches, BetType.FulltimeOneXTwo, "2"),
                        DrawFulltimeOneXTwo = SetTotalTransAndStakeValues(matches, BetType.FulltimeOneXTwo, "x"),
                        HomeFirstHalfHandicap = SetTotalTransAndStakeValues(matches, BetType.FirstHalfHandicap, "h"),
                        AwayFirstHalfHandicap = SetTotalTransAndStakeValues(matches, BetType.FirstHalfHandicap, "a"),
                        HomeFirstHalfOverUnder = SetTotalTransAndStakeValues(matches, BetType.FirstHalfOverUnder, "h"),
                        AwayFirstHalfOverUnder = SetTotalTransAndStakeValues(matches, BetType.FirstHalfOverUnder, "a"),
                        HomeFirstHalfOddEven = SetTotalTransAndStakeValues(matches, BetType.FirstHalfOddEven, "h"),
                        AwayFirstHalfOddEven = SetTotalTransAndStakeValues(matches, BetType.FirstHalfOddEven, "a"),
                        HomeFirstHalfOneXTwo = SetTotalTransAndStakeValues(matches, BetType.FirstHalfOneXTwo, "1"),
                        AwayFirstHalfOneXTwo = SetTotalTransAndStakeValues(matches, BetType.FirstHalfOneXTwo, "2"),
                        DrawFirstHalfOneXTwo = SetTotalTransAndStakeValues(matches, BetType.FirstHalfOneXTwo, "x"),
                        HomeFulltimeMoneyLine = SetTotalTransAndStakeValues(matches, BetType.FulltimeMoneyLine, "h"),
                        AwayFulltimeMoneyLine = SetTotalTransAndStakeValues(matches, BetType.FulltimeMoneyLine, "a")
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
