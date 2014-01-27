// ---------------------------------------------------------------------------------
// <copyright file="LASSystemParlayDetail.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Jacob.Phan" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core;
    using Fanex.BetList.Core.Builder.SystemParlayBuilder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// Build html for system parlay ticket detail.
    /// </summary>
    public class LASSystemParlayDetail : SystemParlayDetail, ILASSystemParlayDetail
    {
        private SystemParlayDetail_Template _template;
        private IList<ISystemParlayTicket> _ticketList;
        private IList<ISystemParlaySerial> _serialList;
        private ITicketHelper _ticketHelper;
        private ISystemParlayData _systemParlayData;

        public LASSystemParlayDetail(ITicketHelper ticketHelper, ISystemParlayData systemParlayData)
        {
            _ticketHelper = ticketHelper;
            _systemParlayData = systemParlayData;
        }

        /// <summary>
        /// Build html.
        /// </summary>
        /// <returns>
        /// The html string is returned to client.
        /// </returns>
        public string Build()
        {
            this.PrepareTemplate();
            this.PrepareTicketListAndSerialList();
            this.RenderRows();
            this.RenderTotalRows();
            this.RenderOutstanding();
            return _template.ToString();
        }

        private void RenderOutstanding()
        {
            _template.Outstanding.Outstanding = Formatter.HighlightNegativeNumber(_systemParlayData.Outstanding);
        }

        private void RenderTotalRows()
        {
            var totalPlayerCommission = _ticketHelper.ShowSystemParlayPlayerComm ? _ticketList.Sum(d => d.PlayerComm) : 0;
            var totalWinLost = _ticketList.Sum(d => d.WinLost) + totalPlayerCommission;
            _template.TotalContents.Visible = _ticketList.Count > 0;
            _template.TotalContents.TotalWinloss = Formatter.HighlightNegativeNumber(totalWinLost);
        }

        /// <summary>
        /// Renders the rows.
        /// </summary>
        private void RenderRows()
        {
            var dateList = _ticketList.Select(d => d.WinLostDate)
                        .Distinct()
                        .OrderBy(d => d)
                        .ToList();

            foreach (var date in dateList)
            {
                this.RenderOneDay(date);
            }

            this.RenderEmptyRowInCaseHaveNoData();
        }

        private void RenderOneDay(DateTime date)
        {
            var dataInDate = _ticketList.Where(d => d.WinLostDate == date).ToList();

            for (var j = 0; j < dataInDate.Count(); j++)
            {
                this.RenderEvent(dataInDate, j);
            }

            this.RenderSubTotalForOneDay(dataInDate);
        }

        /// <summary>
        /// Renders the event.
        /// </summary>
        /// <param name="dataInDate">The data in date.</param>
        /// <param name="eventOrderInDay">The event order in day.</param>
        private void RenderEvent(List<ISystemParlayTicket> dataInDate, int eventOrderInDay)
        {
            var row = dataInDate[eventOrderInDay];

            // Rows
            var rowBlock = new SystemParlayDetail_Contents_Block();
            rowBlock.WinlossDate.RowSpan = dataInDate.Count().ToString();
            rowBlock.WinlossDate.WinlossDate = string.Format("{0:MM/dd/yyyy}", row.WinLostDate);
            rowBlock.WinlossDate.Visible = eventOrderInDay == 0;
            rowBlock.Status = GetBetListStatusName(row.StatusId);
            rowBlock.Winloss = Formatter.HighlightNegativeNumber(row.WinLost);
            rowBlock.Odds = row.MOdds == 0 ? "-" : Formatter.HighlightNegativeNumber(row.MOdds);
            rowBlock.Stake = Formatter.DecFormat(row.Stake, 2);
            rowBlock.PlayerCommission.Visible = _ticketHelper.ShowSystemParlayPlayerComm;
            rowBlock.PlayerCommission.PlayerCommission = Formatter.DecFormat(row.PlayerComm, 2);

            // Event
            rowBlock.Match.Clear();
            var eventDataRows = _serialList.Where(d => d.Serial == row.Serial).ToList();
            if (eventDataRows.Count > 0)
            {
                foreach (var eventRow in eventDataRows)
                {
                    var matchBlock = new SystemParlayDetail_Contents_Match_Block();
                    matchBlock.HomeTeam = _ticketHelper.GetTeamNameById(eventRow.HomeId);
                    matchBlock.AwayTeam = _ticketHelper.GetTeamNameById(eventRow.AwayId);
                    rowBlock.Match.Append(matchBlock);
                }
            }
            else
            {
                rowBlock.Match.SetValue(Fanex.BetList.Core.App_GlobalResources.CoreLang.TotalLosingTicket);
            }

            _template.Contents.Append(rowBlock);
        }

        private void RenderSubTotalForOneDay(List<ISystemParlayTicket> dataInDate)
        {
            var subTotalBlock = new SystemParlayDetail_SubTotalContents_Block();
            var subTotalPlayerCommission = _ticketHelper.ShowSystemParlayPlayerComm ? dataInDate.Sum(d => d.PlayerComm) : 0;
            var subTotal = dataInDate.Sum(d => d.WinLost) + subTotalPlayerCommission;
            subTotalBlock.TotalWinloss = Formatter.HighlightNegativeNumber(subTotal);
            _template.Contents.Append(subTotalBlock);
        }

        /// <summary>
        /// Renders the empty row in case have no data.
        /// </summary>
        private void RenderEmptyRowInCaseHaveNoData()
        {
            if (_ticketList.Count == 0)
            {
                var emptycontent = new SystemParlayDetail_Contents_Block
                {
                    Stake = string.Empty,
                    Winloss = string.Empty,
                    WinlossDate = new SystemParlayDetail_Contents_WinlossDate_Block { WinlossDate = string.Empty },
                    Odds = string.Empty,
                    Status = string.Empty
                };

                emptycontent.Match.Hide();
                _template.Contents.Append(emptycontent);
            }
        }

        private void PrepareTicketListAndSerialList()
        {
            _ticketList = _systemParlayData.TicketList;
            _serialList = _systemParlayData.SerialList;
        }

        private void PrepareTemplate()
        {
            _template = new SystemParlayDetail_Template();
            _template.Contents.Clear();
            _template.SubTotalContents.Clear();
        }
    }
}