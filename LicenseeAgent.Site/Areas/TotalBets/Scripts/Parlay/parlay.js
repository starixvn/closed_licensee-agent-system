/*global core, language, fanex:true, pageContext*/
'use strict';

var totalBets = totalBets || {};
totalBets.parlay = (function ($this, $) {
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
        pagerOption: {
            itemsPerPage: core.pageSizes[2],
            currentPage: 0,
            itemsPerPageArray: core.pageSizes
        },
        showPager: true,
        enableHiddenColumns: false,
        autoResizeColumn: true
    };

    var reRenderAfterPaged = function (betType) {
        var params = {
            BetType: betType
        };
        core.ajaxJSON({
            url: core.resolveUrl('~/BetList/Parlay'),
            data: params
        }, function (data) {
            fanex.betlist.core.reRenderAfterPagedSuccessfully(gridOptions, data);
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
} (totalBets.parlay || {}, jQuery));