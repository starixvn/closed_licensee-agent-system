/*global core, language, fanex:true, pageContext*/
var gridOptions = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center", CanResize: false },
                { HeaderText: language.getString("Member"), Width: 95, Name: "Member", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: language.getString("TransTime"), Width: 90, Name: "TransTime", HeaderAlign: "center", CellAlign: "center", CanResize: false },
                { HeaderText: language.getString("Choice"), Width: 335, Name: "Choice", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false },
                { HeaderText: language.getString("AgentPT"), Width: 75, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right", CanResize: false },
                { HeaderText: language.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center", CanResize: false }
            ],
    showPager: true,
    enableHiddenColumns: false,
    gridExpandHeight: 100,
    bodyRowHeight: 29,
    autoResizeColumn: true
};

var fanex = fanex || {};
fanex.totalBets = fanex.totalBets || {};
fanex.totalBets.parlay = {
    init: function () {
        'use strict';
        $('.selectpicker').selectpicker();

        var hiddenColumns = [];
        if (pageContext.LegendType === 1) {
            hiddenColumns.push(9);
        }
        if (pageContext.IsLastLevel) {
            hiddenColumns.push(10);
        }
        gridOptions.hiddenColumns = hiddenColumns;

        // search
        $('#search-form').submit(function (e) {
            fanex.totalBets.parlay.reRenderAfterPaged(0, core.pageSizes[0]);
            e.preventDefault();
        });

        $("#mixParlayTab").click(function () {
            fanex.totalBets.parlay.switchParlayType(true);
        });

        $("#systemParlayTab").click(function () {
            fanex.totalBets.parlay.switchParlayType(false);
        });

        // get total bets mix parlay 
        fanex.totalBets.parlay.switchParlayType(true);
    },
    reRenderAfterPaged: function (currentPage, itemsPerPage, betType) {
        'use strict';
        if (gridOptions && gridOptions.pagerOption) {
            currentPage = currentPage >= 0 ? currentPage
                                                : gridOptions.pagerOption.currentPage;
            itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
        }

        var params = {
            BetType: betType
        };

        $.ajax({
            type: 'post',
            data: params,
            beforeSend: function () {
                core.blockUI();
            },
            success: function (data) {
                fanex.betlist.core.reRenderAfterPagedSuccessfully(gridOptions, data);
            },
            complete: function () {
                core.unblockUI();
            }
        });
    },
    switchParlayType: function (isMixParlay) {
        'use strict';
        if (isMixParlay) {
            $("#mixParlayTab").removeClass('inactive-tab');
            $("#mixParlayTab").addClass('active-tab');
            $("#systemParlayTab").removeClass('active-tab');
            $("#systemParlayTab").addClass('inactive-tab');
            fanex.totalBets.parlay.reRenderAfterPaged(0, core.pageSizes[0], 9);
        }
        else {
            $("#mixParlayTab").removeClass('active-tab');
            $("#mixParlayTab").addClass('inactive-tab');
            $("#systemParlayTab").removeClass('inactive-tab');
            $("#systemParlayTab").addClass('active-tab');
            fanex.totalBets.parlay.reRenderAfterPaged(0, core.pageSizes[0], 29);
        }
    }
};

$(function () {
    'use strict';
    fanex.totalBets.parlay.init();
});