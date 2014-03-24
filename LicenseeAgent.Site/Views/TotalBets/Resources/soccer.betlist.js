/*global CORE, LANGUAGE*/

var gridOptions = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("Member"), Width: 95, Name: "Member", HeaderAlign: "center", CellAlign: "left", CanResize: true },
                { HeaderText: LANGUAGE.getString("TransTime"), Width: 100, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("Choice"), Width: 345, Name: "Choice", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Stake"), Width: 70, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("WinLoss"), Width: 60, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("AgentPT"), Width: 60, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
            ],
    showPager: true,
    gridExpandHeight: 60,
    bodyRowHeight: 29,
    autoResizeColumn: true
};

var SoccerBetList = (function ($) {
    /*jshint strict:false */
    return {
        sportType: 1, //Soccer sporttype
        init: function () {
            CORE.blockUI();
            SoccerBetList.showBetList();
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
                url: CORE.resolveUrl("~/Totalbets/GetSoccerMoreBetList"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                        return;
                    }

                    fanex.betlist.core.reRenderAfterPagedSuccessfully(gridOptions, data, [10]);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        }
    };
} (jQuery));

$(function () {
    /*jshint strict:false */
    SoccerBetList.init();
});