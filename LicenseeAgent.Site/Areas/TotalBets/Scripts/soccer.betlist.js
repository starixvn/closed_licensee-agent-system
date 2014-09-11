/*global core, language, pageContext*/

var totalBets = totalBets || {};
totalBets.soccerBetList = (function ($) {
    'use strict';
    var gridOptions = {
        columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center", CanResize: false },
                { HeaderText: language.getString("Member"), Width: 95, Name: "Member", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: language.getString("TransTime"), Width: 100, Name: "TransTime", HeaderAlign: "center", CellAlign: "center", CanResize: false },
                { HeaderText: language.getString("Choice"), Width: 345, Name: "Choice", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false },
                { HeaderText: language.getString("AgentPT"), Width: 60, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center", CanResize: false }
            ],
        showPager: true,
        enableHiddenColumns: false,
        gridExpandHeight: 60,
        bodyRowHeight: 29,
        autoResizeColumn: true
    };

    return {
        sportType: 1, //Soccer sporttype
        init: function () {
            core.blockUI();

            var hiddenColumns = [];
            if (pageContext.LegendType === 1) {
                hiddenColumns.push(9);
            }
            if (pageContext.IsLastLevel) {
                hiddenColumns.push(10);
            }
            gridOptions.hiddenColumns = hiddenColumns;

            totalBets.soccerBetList.showBetList();
        },

        showBetList: function () {
            var matchId = $('#soccer-more-betlist').attr("data-matchid");
            var bettype = $('#soccer-more-betlist').attr("data-bettype");
            var betteam = $('#soccer-more-betlist').attr("data-betteam");
            var params = {
                MatchId: matchId,
                Bettype: bettype,
                Betteam: betteam
            };

            $.ajax({
                url: core.resolveUrl("~/Totalbets/Soccer/GetSoccerMoreBetList"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                        return;
                    }

                    fanex.betlist.core.reRenderAfterPagedSuccessfully(gridOptions, data);
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        }
    };
} (jQuery));