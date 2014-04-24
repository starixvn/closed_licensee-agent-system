/*global popup, language, core, fanex:true*/
/* exported OpenIPInfo, showMP, showCombMP, ViewResult */
"use strict";

var showMixParlayDetailUrl = "~/BetList/MixParlayDetails?transid={0}&sportType={1}&betType={2}";
var showBetListDetailsUrl = "~/BetList/Details?transId={0}&status={1}&SportType={2}&BetType={3}";
var showBetListResultsUrl = "~/BetList/Results";
var showSystemParlayDetailsUrl = "~/BetList/SystemParlayResult?refNo=";
var sportTypeParlay = 0;
var betTypeMixParlay = 9;
var betTypeSystemParlay = 29;

function OpenIPInfo(ipstring) {
    fanex.betlist.core.getIpInformation(ipstring);
}

function showMP(transId) {
    var divEvent = $('#divEvent_' + transId);
    fanex.betlist.core.showMixParlayDetails(transId, sportTypeParlay, betTypeMixParlay, divEvent);
}

function showCombMP(transId, index) {
    var divEvent = $('#divEvent_' + transId + "" + index);
    fanex.betlist.core.showMixParlayDetails(transId, sportTypeParlay, betTypeSystemParlay, divEvent);
}

var fanex = fanex || {};
fanex.betlist = fanex.betlist || {};
fanex.betlist.core = {
    reRenderAfterPagedSuccessfully: function (gridOptions, data, hidedColumn) {
        gridOptions.pagerOption = {
            itemsPerPage: core.pageSizes[0],
            itemsPerPageArray: core.pageSizes,
            currentPage: 0,
            showDetail: true
        };
        gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
        $('#BetListGrid').show().fanexGrid(gridOptions);

        if (data.IsLastLevel === true) {
            $("#BetListGrid").fanexGrid("hideColumns", gridOptions, hidedColumn);
        }
    },
    getIpInformation: function (ipAddress) {
        var urlIpInformation = $("#BetListGrid").data('url-ip');
        $.ajax({
            url: urlIpInformation,
            type: 'post',
            data: { IpAddress: ipAddress },
            beforeSend: function () {
                core.blockUI();
            },
            success: function (data) {
                if (data != null) {
                    popup.open({
                        title: language.getString("IpInformation"),
                        message: data,
                        clickout: true
                    });
                }
            },
            complete: function () {
                core.unblockUI();
            }
        });
    },
    showDetails: function (transId, status, sportType, betType) {
        var url = showBetListDetailsUrl.format(transId, status, sportType, betType);
        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: core.resolveUrl(url),
            success: function (response) {
                popup.open({
                    title: language.getString("BetListDetails") + transId,
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
        var loadMixParlayDetail = function (transId, sportType, betType) {
            var url = showMixParlayDetailUrl.format(transId, sportType, betType);
            $.ajax({
                type: "GET",
                contentType: "text/html",
                cache: false,
                url: core.resolveUrl(url),
                success: function (response) {
                    divEvent.html(response);
                    $(".line:last", divEvent).remove();
                    $('#BetListGrid').fanexGrid('resizeGrid');
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
            $('#BetListGrid').fanexGrid('resizeGrid');
        }
    },
    showSystemParlayDetails: function (refNo, custId, betTeam) {
        var url = showSystemParlayDetailsUrl + refNo + "&custId=" + custId;
        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: core.resolveUrl(url),
            success: function (response) {
                popup.open({
                    title: betTeam,
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
    showResults: function (matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay) {
        var url = bettype === betTypeMixParlay || bettype === betTypeSystemParlay
                ? showBetListResultsUrl + "?refNo=" + refnoMixParlay + "&isSportsbook=false&sportType=" + sporttype
                : showBetListResultsUrl + "?matchId=" + matchid + "&isSportsbook=true&sportType=" + sporttype;

        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: core.resolveUrl(url),
            success: function (response) {
                popup.open({
                    title: language.getString("Results"),
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
    }
};

var ViewResult = fanex.betlist.core.showResults;