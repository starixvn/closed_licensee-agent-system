namespace LicenseeAgent.BetList.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using LicenseeAgent.BetList.Criteria;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class BetListDataAccess : IBetListDataAccess
    {
        /// <summary>
        /// Get Bet List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        public TicketResult GetBetList(ICriteria criteria)
        {
            var result = new TicketResult();
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                var dataset = objectDb.ExecuteDataSet(param);

                // Main ticket
                var mainTable = dataset.Tables.Count > 0 ? dataset.Tables[0] : new DataTable();
                result.MainTicketList = ReadMainTicketData(mainTable, string.Empty);

                // Reference ticket
                var referenceTable = dataset.Tables.Count > 1 ? dataset.Tables[1] : new DataTable();
                result.TicketDataList = ReadReferenceTicket(referenceTable, string.Empty);
            }

            return result;
        }

        /// <summary>
        /// Get Bet List Parlay.
        /// </summary>
        /// <param name="criteria">Mix Parlay Details Criteria.</param>
        /// <returns>Ticket results.</returns>
        public TicketResult GetMixParlayDetails(MixParlayDetailsCriteria criteria)
        {
            var result = new TicketResult();
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                var dataset = objectDb.ExecuteDataSet(param);

                var mainTable = dataset.Tables.Count > 0 ? dataset.Tables[0] : new DataTable();
                result.MainTicketList = ReadMainTicketData(mainTable, string.Empty);
            }

            return result;
        }

        /// <summary>
        /// Get Parlay List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        public TicketResult GetParlayList(ICriteria criteria)
        {
            var result = new TicketResult();
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                var dataset = objectDb.ExecuteDataSet(param);

                var mainTable = dataset.Tables.Count > 0 ? dataset.Tables[0] : new DataTable();
                result.MainTicketList = ReadMainTicketData(mainTable, string.Empty);
            }

            return result;
        }

        /// <summary>
        /// Get system parlay result data.
        /// </summary>
        /// <param name="criteria">System parlay result criteria.</param>
        /// <returns>System Parlay Data.</returns>
        public SystemParlayData GetSystemParlayResult(SystemParlayResultCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var systemParlayData = new SystemParlayData();
                using (var multipleQuery = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var ticketList = multipleQuery.Read<SystemParlayTicket>();
                    var serialList = multipleQuery.Read<SystemParlaySerial>();

                    systemParlayData.TicketList = ticketList.Cast<ISystemParlayTicket>().ToList();
                    systemParlayData.SerialList = serialList.Cast<ISystemParlaySerial>().ToList();
                }

                systemParlayData.Outstanding = Converter.ToDecimal(objectDb.GetOuputParams()["outstanding"]);
                return systemParlayData;
            }
        }

        #region BetList.Utilities

        /// <summary>
        /// Read main ticket data.
        /// </summary>
        /// <param name="dataTable">Data table Parameter value.</param>
        /// <param name="baseCurrency"> Parameter value.</param>
        /// <returns>Result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Review")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Fix later.")]
        private static List<LasTicket> ReadMainTicketData(DataTable dataTable, string baseCurrency)
        {
            var playerCommCol = "playercomm" + baseCurrency;
            var commCol = "comm" + baseCurrency;
            var actualStakeCol = "actual_stake" + baseCurrency;

            var columnCollection = dataTable.Columns;

            // string
            var containsUsername = columnCollection.Contains("username");
            var containsUsername2 = columnCollection.Contains("username2");
            var containsRefNo = columnCollection.Contains("refno");
            var containsMatchCode = columnCollection.Contains("matchcode");
            var containsStatus = columnCollection.Contains("status");
            var containsBetTeam = columnCollection.Contains("betteam");
            var containsBetStatus = columnCollection.Contains("betstatus");
            var containsShowTime = columnCollection.Contains("showtime");
            var containsOddsType = columnCollection.Contains("oddstype");
            var containsExternalId = columnCollection.Contains("nickname");
            var containsRace = columnCollection.Contains("race");
            var containsIP = columnCollection.Contains("ip");

            // Boolean
            var containsLiveIndicator = columnCollection.Contains("liveindicator");
            var containsNeutral = columnCollection.Contains("isneutral");

            // Int
            var containsSportType = columnCollection.Contains("sporttype");
            var containsBetType = columnCollection.Contains("bettype");
            var containsLeagueId = columnCollection.Contains("leagueid");
            var containsHomeId = columnCollection.Contains("homeid");
            var containsAwayId = columnCollection.Contains("awayid");
            var containsMatchId = columnCollection.Contains("matchid");
            var containsCustId = columnCollection.Contains("custid");
            var containsLiveHomeScore = columnCollection.Contains("livehomescore");
            var containsLiveAwayScore = columnCollection.Contains("liveawayscore");
            var containsStatusID = columnCollection.Contains("statusid");

            // Long
            var containsTransId = columnCollection.Contains("transid");
            var containsBetId = columnCollection.Contains("betid");

            // DateTime
            var containsWinlostDate = columnCollection.Contains("winlostdate");
            var containsTransDate = columnCollection.Contains("transdate");

            // Money
            var containsPlayerComm = columnCollection.Contains(playerCommCol);
            var containsComm = columnCollection.Contains(commCol);
            var containsWinlost = columnCollection.Contains("winlost");
            var containsStake = columnCollection.Contains("stake");
            var containsActualStake = columnCollection.Contains(actualStakeCol);
            var containsOdds = columnCollection.Contains("odds");
            var containsMasterOdds = columnCollection.Contains("modds");
            var containsHandicap1 = columnCollection.Contains("hdp1");
            var containsHandicap2 = columnCollection.Contains("hdp2");

            // Agent PT & Commission
            var containsAgentPT = columnCollection.Contains("agentpt");
            var containsAgentPTPercent = columnCollection.Contains("agentptpercent");
            var containsAgentComm = columnCollection.Contains("agentcomm");
            var containsAgentCommPercent = columnCollection.Contains("agentcommpercent");

            var result = new List<LasTicket>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var ticket = new LasTicket();

                // string
                ticket.UserName = containsUsername ? Converter.ToString2(dataRow["username"]) : string.Empty;
                ticket.UserName2 = containsUsername2 ? Converter.ToString2(dataRow["username2"]) : string.Empty;
                ticket.RefNo = containsRefNo ? Converter.ToString2(dataRow["refno"]) : string.Empty;
                ticket.MatchCode = containsMatchCode ? Converter.ToString2(dataRow["matchcode"]) : string.Empty;
                ticket.Status = containsStatus ? Converter.ToString2(dataRow["status"]) : string.Empty;
                ticket.BetTeam = containsBetTeam ? Converter.ToString2(dataRow["betteam"]) : string.Empty;
                ticket.EventStatus = containsBetStatus ? Converter.ToString2(dataRow["betstatus"]) : string.Empty;
                ticket.ShowTime = containsShowTime ? Converter.ToString2(dataRow["showtime"]) : string.Empty;
                ticket.OddsType = containsOddsType ? Converter.ToString2(dataRow["oddstype"]) : "0";
                ticket.ExternalId = containsExternalId ? Converter.ToString2(dataRow["nickname"]) : string.Empty;
                ticket.Race = containsRace ? Converter.ToString2(dataRow["race"]) : string.Empty;
                ticket.IP = containsIP ? Converter.ToString2(dataRow["ip"]) : string.Empty;

                // Boolean
                ticket.IsLive = containsLiveIndicator && ReadBoolean(dataRow["liveindicator"]);
                ticket.IsNeutral = containsNeutral && ReadBoolean(dataRow["isneutral"]);

                // Int
                ticket.SportTypeId = containsSportType ? Converter.ToInt(dataRow["sporttype"]) : 0;
                ticket.BetTypeId = containsBetType ? Converter.ToInt(dataRow["bettype"]) : 0;
                ticket.LeagueId = containsLeagueId ? Converter.ToInt(dataRow["leagueid"]) : 0;
                ticket.HomeId = containsHomeId ? Converter.ToInt(dataRow["homeid"]) : 0;
                ticket.AwayId = containsAwayId ? Converter.ToInt(dataRow["awayid"]) : 0;
                ticket.MatchId = containsMatchId ? Converter.ToInt(dataRow["matchid"]) : 0;
                ticket.CustId = containsCustId ? Converter.ToInt(dataRow["custid"]) : 0;
                ticket.LiveHomeScore = containsLiveHomeScore ? Converter.ToInt(dataRow["livehomescore"]) : 0;
                ticket.LiveAwayScore = containsLiveAwayScore ? Converter.ToInt(dataRow["liveawayscore"]) : 0;
                ticket.StatusId = containsStatusID ? Converter.ToInt(dataRow["statusid"]) : 0;

                // Long
                ticket.TransId = containsTransId ? Convert.ToInt64(dataRow["transid"]) : 0;
                ticket.BetId = containsBetId && !Convert.IsDBNull(dataRow["betid"]) ? new long?(Convert.ToInt64(dataRow["betid"])) : null;

                // DateTime
                ticket.WinlostDate = containsWinlostDate ? Converter.ToDateTime(dataRow["winlostdate"]) : DateTime.Now;
                ticket.TransDate = containsTransDate ? Converter.ToDateTime(dataRow["transdate"]) : DateTime.Now;

                // Money
                ticket.PlayerComm = containsPlayerComm ? Converter.ToDecimal(dataRow[playerCommCol]) : 0;
                ticket.Comm = containsComm ? Converter.ToDecimal(dataRow[commCol]) : 0;
                ticket.Winlost = containsWinlost ? Converter.ToDecimal(dataRow["winlost"]) : 0;
                ticket.Stake = containsStake ? Converter.ToDecimal(dataRow["stake"]) : 0;
                ticket.ActualStake = containsActualStake ? Converter.ToDecimal(dataRow[actualStakeCol]) : 0;
                ticket.Odds = containsOdds ? Converter.ToDecimal(dataRow["odds"]) : 0;
                ticket.MasterOdds = containsMasterOdds ? Converter.ToDecimal(dataRow["modds"]) : 0;
                ticket.Handicap1 = containsHandicap1 ? Converter.ToDecimal(dataRow["hdp1"]) : 0;
                ticket.Handicap2 = containsHandicap2 ? Converter.ToDecimal(dataRow["hdp2"]) : 0;

                // Agent PT & Commission
                ticket.AgentPositionTaking = containsAgentPT ? Converter.ToDecimal(dataRow["agentpt"]) : 0;
                ticket.AgentPTPercent = containsAgentPTPercent ? Converter.ToDecimal(dataRow["agentptpercent"]) : 0;
                ticket.AgentComm = containsAgentComm ? Converter.ToDecimal(dataRow["agentcomm"]) : 0;
                ticket.AgentCommPercent = containsAgentCommPercent ? Converter.ToDecimal(dataRow["agentcommpercent"]) : 0;

                result.Add(ticket);
            }

            return result;
        }

        /// <summary>
        /// Read reference ticket.
        /// </summary>
        /// <param name="data">Data Parameter value.</param>
        /// <param name="baseCurrency"> Parameter value.</param>
        /// <returns>Result value.</returns>
        private static List<ITicketData> ReadReferenceTicket(DataTable data, string baseCurrency)
        {
            var result = new List<ITicketData>();
            var containsBetTeam = data.Columns.Contains("betteam");
            var containsStake = data.Columns.Contains("stake");
            foreach (DataRow dataRow in data.Rows)
            {
                var ticket = new LasTicketData();

                // string
                ticket.BetTeam = containsBetTeam ? dataRow["betteam"].ToString() : string.Empty;
                ticket.RefNo = dataRow["refno"].ToString();
                ticket.Status = dataRow["status"].ToString();

                // Int
                ticket.BetTypeId = Convert.ToInt32(dataRow["bettype"]);

                // Money
                ticket.Stake = containsStake ? Convert.ToDecimal(dataRow["stake" + baseCurrency]) : 0;

                // Decimal
                ticket.Odds = Convert.ToDecimal(dataRow["odds"]);

                ticket.AgentPTPercent = Convert.ToDecimal(dataRow["agentptpercent"]);
                ticket.AgentCommPercent = Convert.ToDecimal(dataRow["agentcommpercent"]);

                result.Add(ticket);
            }

            return result;
        }

        /// <summary>
        /// Read boolean object.
        /// </summary>
        /// <param name="value"> Parameter value.</param>
        /// <returns>Result value.</returns>
        private static bool ReadBoolean(object value)
        {
            if (value == null || value == Convert.DBNull)
            {
                return false;
            }

            if (value is bool)
            {
                return Convert.ToBoolean(value);
            }

            if (value is int)
            {
                return Convert.ToInt32(value) > 0;
            }

            return false;
        }

        #endregion BetList.Utilities
    }
}