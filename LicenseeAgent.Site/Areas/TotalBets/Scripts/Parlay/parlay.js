/*global CORE, LANGUAGE, GRID, BET_LIST, pageContext*/
'use strict';

var TOTAL_BETS = TOTAL_BETS || {};
TOTAL_BETS.PARLAY = (function ($this, $) {
    var gridOptions = {
        columns: [
            { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString("Member"), Width: 95, Name: "Member", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: LANGUAGE.getString("TransTime"), Width: 90, Name: "TransTime", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString("Choice"), Width: 335, Name: "Choice", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString("AgentPT"), Width: 75, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center", CanResize: false }
        ],
        pagerOption: {
            itemsPerPage: GRID.PageSizes[2],
            currentPage: 0,
            itemsPerPageArray: GRID.PageSizes
        },
        showPager: true,
        enableHiddenColumns: false,
        autoResizeColumn: true
    };

    var reRenderAfterPaged = function (betType) {
        var params = {
            BetType: betType
        };
        CORE.ajaxJSON({
            url: CORE.resolveUrl('~/BetList/Parlay'),
            data: params
        }, function (data) {
            BET_LIST.reRenderAfterPagedSuccessfully(gridOptions, data);
        });
    };

    $this.init = function () {
        var hiddenColumns = [];
        if (pageContext.LegendType === 1) {
            hiddenColumns.push(9);
        }
        if (pageContext.IsLastLevel) {
            hiddenColumns.push(10);
        }
        gridOptions.hiddenColumns = hiddenColumns;

        $('.btn-parlay').click(function () {
            $('.btn-parlay').removeClass('active');
            $(this).addClass('active');
            var betType = $(this).data('bet-type');
            reRenderAfterPaged(betType);
        });

        $('.btn-parlay.active').click();
    };

    return $this;
} (TOTAL_BETS.PARLAY || {}, jQuery));