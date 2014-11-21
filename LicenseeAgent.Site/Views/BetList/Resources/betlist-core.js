/*global popup, language, core, fanex:true*/
/* exported OpenIPInfo, showMP, showCombMP, ViewResult */

var sportTypeParlay = 0;
var betTypeMixParlay = 9;
var betTypeSystemParlay = 29;

function OpenIPInfo(ipstring) {
    'use strict';
    fanex.betlist.core.getIpInformation(ipstring);
}

function showMP(transId) {
    'use strict';
    var divEvent = $('#divEvent_' + transId);
    fanex.betlist.core.showMixParlayDetails(transId, sportTypeParlay, betTypeMixParlay, divEvent);
}

function showCombMP(transId, index) {
    'use strict';
    var divEvent = $('#divEvent_' + transId + "" + index);
    fanex.betlist.core.showMixParlayDetails(transId, sportTypeParlay, betTypeSystemParlay, divEvent);
}

var fanex = fanex || {};
fanex.betlist = fanex.betlist || {};
fanex.betlist.core = {
    reRenderAfterPagedSuccessfully: function (gridOptions, data, hasTotalRow) {
        'use strict';
        gridOptions.pagerOption = {
            itemsPerPage: core.pageSizes[0],
            itemsPerPageArray: core.pageSizes,
            currentPage: 0,
            showDetail: true
        };
        if (hasTotalRow === true) {
            gridOptions.showFooter = true;
            if (core.getQueryParamValue('hideFooter') === 'true') {
                gridOptions.showFooter = false;
            }

            var footerTitle = '<div>{0}:</div><div>{1}:</div><div>{2}:</div>'.format(
                language.getString('SubTotalWinLost'),
                language.getString('SubTotalComm'),
                language.getString('Total')
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
        gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
        $('.freegrid').show().fanexGrid(gridOptions);
    },
    getIpInformation: function (ipAddress) {
        'use strict';
        core.common.showIP(ipAddress);
    },
    showDetails: function (transId, status, sportType, betType) {
        'use strict';
        var url = "~/BetList/Details?transId={0}&status={1}&SportType={2}&BetType={3}".format(transId, status, sportType, betType);
        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: core.resolveUrl(url),
            success: function (response) {
                popup.open({
                    title: language.getString("BetListDetails") + ' ' + transId,
                    message: response
                });
            },
            beforeSend: function () {
                core.blockUI();
            },
            complete: function () {
                core.unblockUI();
            }
        });
    },
    showMixParlayDetails: function (transId, sportType, betType, divEvent) {
        'use strict';
        var loadMixParlayDetail = function (transId, sportType, betType) {
            var url = "~/BetList/MixParlayDetails?transid={0}&sportType={1}&betType={2}".format(transId, sportType, betType);
            $.ajax({
                type: "GET",
                contentType: "text/html",
                cache: false,
                url: core.resolveUrl(url),
                success: function (response) {
                    divEvent.html(response);
                    $(".line:last", divEvent).remove();
                    $('.freegrid').fanexGrid('resizeGrid');
                },
                beforeSend: function () {
                    core.blockUI();
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        };

        divEvent.toggle();
        var dataExisted = $("#divEvent_" + transId).text().length > 0;
        if (!dataExisted) {
            loadMixParlayDetail(transId, sportType, betType);
        } else {
            $('.freegrid').fanexGrid('resizeGrid');
        }
    },
    showSystemParlayDetails: function (refNo, transId, betTeam) {
        'use strict';
        var url = "~/BetList/SystemParlayResult?refNo=" + refNo;
        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: core.resolveUrl(url),
            success: function (response) {
                popup.open({
                    title: betTeam + ' - RefNo: ' + transId,
                    message: response
                });
            },
            beforeSend: function () {
                core.blockUI();
            },
            complete: function () {
                core.unblockUI();
            }
        });
    },
    showResults: function (matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright) {
        'use strict';
        var data = {
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
        };
        var url = '~/BetList/Results?' + $.param(data);

        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: core.resolveUrl(url),
            success: function (response) {
                popup.open({
                    title: language.getString("Results"),
                    message: response,
                    height: 200
                });
            },
            beforeSend: function () {
                core.blockUI();
            },
            complete: function () {
                core.unblockUI();
            }
        });
    }
};

var ViewResult = fanex.betlist.core.showResults;