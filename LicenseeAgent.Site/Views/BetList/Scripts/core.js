/*global CORE, LANGUAGE, POPUP, GRID, PAGE*/
/* exported OpenIPInfo, showMP, showCombMP, ViewResult */
'use strict';

var BET_LIST = (function ($this, $) {
    var gridOptions = {
        columns: [
            { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
            { HeaderText: LANGUAGE.getString("TransTime"), Width: 90, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: LANGUAGE.getString("Choice"), Width: 335, Name: "Choice", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: LANGUAGE.getString("AgentPT"), Width: 75, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
        ],
        showPager: true,
        pagerOption: {
            currentPage: 0,
            itemsPerPage: GRID.PageSizes[0],
            itemsPerPageArray: GRID.PageSizes,
            showDetail: true
        },
        enableHiddenColumns: false
    };

    $this.reRenderAfterPagedSuccessfully = function (gridOptions, data, hasTotalRow) {
        gridOptions.pagerOption = {
            itemsPerPage: GRID.PageSizes[0],
            itemsPerPageArray: GRID.PageSizes,
            currentPage: 0,
            showDetail: true
        };
        if (hasTotalRow === true) {
            gridOptions.showFooter = true;
            if (CORE.getQueryParamValue('hideFooter') === 'true') {
                gridOptions.showFooter = false;
            }

            var footerTitle = '<div>{0}:</div><div>{1}:</div><div>{2}:</div>'.format(
                LANGUAGE.getString('SubTotalWinLost'),
                LANGUAGE.getString('SubTotalComm'),
                LANGUAGE.getString('Total')
            );
            var footerValue = '<div>{0}</div><div>{1}</div><div>{2}</div>'.format(
                data.SubTotalWinLost.formatNegative(),
                data.SubTotalComm.formatNegative(),
                data.TotalWinLost.formatNegative()
            );
            gridOptions.footerRows = [
                {
                    Columns: [
                        { Value: footerTitle, Name: "Index", CellAlign: "right", MergeColNumber: 5 },
                        { Value: footerValue, Name: "WinLoss", CellAlign: "right" }
                    ]
                }
            ];
        }
        gridOptions.bodyRows = GRID.generateBodyRows(data.List, gridOptions);
        $('.freegrid').show().fanexGrid(gridOptions);
    };

    $this.showByMember = function (setting) {
        var grid;
        if (typeof setting.Container === 'undefined') {
            grid = $('.freegrid');
        } else {
            grid = $(setting.Container);
        }
        CORE.ajaxJSON({
            url: CORE.resolveUrl('~/BetList/Member'),
            data: setting.Input
        }, function (data) {
            var hiddenColumns = [];
            if (data.LegendType === 1) {
                hiddenColumns.push(8);
            }
            if (data.IsLastLevel) {
                hiddenColumns.push(9);
            }
            gridOptions.hiddenColumns = hiddenColumns;
            gridOptions.bodyRows = GRID.generateBodyRows(data.List, gridOptions);
            GRID.generateTotalRows(gridOptions.bodyRows, gridOptions);

            if (setting.ShowTotalRow) {
                gridOptions.showFooter = true;
                var footerTitle = '<div>{0}:</div><div>{1}:</div><div>{2}:</div>'.format(
                    LANGUAGE.getString('SubTotalWinLost'),
                    LANGUAGE.getString('SubTotalComm'),
                    LANGUAGE.getString('Total')
                );
                var footerValue = '<div>{0}</div><div>{1}</div><div>{2}</div>'.format(
                    data.SubTotalWinLost.formatNegative(),
                    data.SubTotalComm.formatNegative(),
                    data.TotalWinLost.formatNegative()
                );
                gridOptions.footerRows = [
                    {
                        Columns: [
                            { Value: footerTitle, Name: "Index", CellAlign: "right", MergeColNumber: 5 },
                            { Value: footerValue, Name: "WinLoss", CellAlign: "right" }
                        ]
                    }
                ];
            }

            grid.show().fanexGrid(gridOptions);
        });
    };

    $this.showDetails = function (transId, status, sportType, betType) {
        var url = CORE.resolveUrl('~/BetList/Details', {
            transId: transId,
            status: status,
            sportType: sportType,
            betType: betType
        });
        CORE.ajaxHTML({
            url: url
        }, function (response) {
            POPUP.open({
                title: LANGUAGE.getString("BetListDetails") + ' ' + transId,
                message: response
            });
        });
    };

    $this.showResults = function (matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright) {
        var url = CORE.resolveUrl('~/BetList/Results', {
            matchId: matchid,
            raceNo: raceNo,
            betType: bettype,
            sportType: sporttype,
            refNo: refno,
            userName: username,
            winlostDate: winlostdate,
            refnoMixParlay: refnoMixParlay,
            leagueId: league,
            isOutright: isOutright
        });
        CORE.ajaxHTML({
            url: url
        }, function (response) {
            POPUP.open({
                title: LANGUAGE.getString("Results"),
                message: response,
                height: 200
            });
        });
    };

    $this.showMixParlayDetails = function (divEvent, transId, betType) {
        divEvent.toggle();
        var dataExisted = $("#divEvent_" + transId).text().length > 0;
        if (dataExisted) {
            $('.freegrid').fanexGrid('resizeGrid');
            return;
        }

        var url = CORE.resolveUrl('~/BetList/MixParlayDetails', {
            transid: transId,
            sportType: 0,
            betType: betType
        });
        CORE.ajaxHTML({
            url: url
        }, function (response) {
            divEvent.html(response);
            $(".line:last", divEvent).remove();
            $('.freegrid').fanexGrid('resizeGrid');
        });
    };

    $this.showSystemParlayDetails = function (refNo, transId, betTeam) {
        var url = CORE.resolveUrl('~/BetList/SystemParlayDetails', {
            refNo: refNo
        });
        CORE.ajaxHTML({
            url: url
        }, function (response) {
            POPUP.open({
                title: betTeam + ' - RefNo: ' + transId,
                message: response
            });
        });
    };

    return $this;
} (BET_LIST || {}, jQuery));

function OpenIPInfo(ipAddress) {
    PAGE.showIP(ipAddress);
}

function showMP(transId) {
    var divEvent = $('#divEvent_' + transId);
    BET_LIST.showMixParlayDetails(divEvent, transId, 9);
}

function showCombMP(transId, index) {
    var divEvent = $('#divEvent_' + transId + "" + index);
    BET_LIST.showMixParlayDetails(divEvent, transId, 29);
}

function ViewResult(matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright) {
    BET_LIST.showResults(matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright);
}