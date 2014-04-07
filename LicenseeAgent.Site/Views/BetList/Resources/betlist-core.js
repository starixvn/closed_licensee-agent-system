/*global popup, language, core, fanex:true*/
/* exported OpenIPInfo, showMP, showCombMP, ViewResult */
"use strict";

var showMixParlayDetailUrl = "~/BetList/MixParlayDetails?transid=";
var showBetListDetailsUrl = "~/BetList/Details?transId=";
var showBetListResultsUrl = "~/BetList/Results";
var showSystemParlayDetailsUrl = "~/BetList/SystemParlayResult?refNo=";

function OpenIPInfo(ipstring) {
    fanex.betlist.core.getIpInformation(ipstring);
}

function showMP(transId) {
    var divEvent = $('#divEvent_' + transId);
    fanex.betlist.core.showMixParlayDetails(transId, divEvent);
}

function showCombMP(transId, index) {
    var divEvent = $('#divEvent_' + transId + "" + index);
    fanex.betlist.core.showMixParlayDetails(transId, divEvent);
}

var ViewResult = fanex.betlist.core.showResults;

var fanex = fanex || {};
fanex.betlist = fanex.betlist || {};
fanex.betlist.core = {
    /*jshint strict:false */
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
        $.ajax({ url: urlIpInformation,
            type: 'post',
            data: { IpAddress: ipAddress },
            beforeSend: function () {
                core.blockUI();
            },
            success: function (data) {
                if (data != null) {
                    var height = 220;
                    var width = 400;
                    var details = '<div class=\"instruction-content\">' + data + '</div>';
                    popup.openDialog(details, language.getString("IpInformation"),
                                     { width: width, height: height, modal: false, clickout: true, openid: $(this).attr('id') });
                    $('.ui-tooltip').remove();
                }
            },
            complete: function () {
                core.unblockUI();
            }
        });
    },
    showDetails: function (transId, status) {
        var url = showBetListDetailsUrl + transId + "&status=" + status;
        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: core.resolveUrl(url),
            /*jshint unused: false*/
            error: function (xhRequest, errorText, thrownError) {
                alert(errorText);
            },
            success: function (response) {
                var height = 280;
                var width = 360;
                var details = '<div>' + response + '</div>';
                var title = language.getString("BetListDetails") + transId;
                popup.openDialog(details, title, { width: width, height: height });
            },
            beforeSend: function () {
                core.blockUI();
            },
            complete: function () {
                core.unblockUI();
            }
        });
    },
    showMixParlayDetails: function (transId, divEvent) {
        var loadMixParlayDetail = function (transId) {
            var url = showMixParlayDetailUrl + transId;
            $.ajax({
                type: "GET",
                contentType: "text/html",
                cache: false,
                url: core.resolveUrl(url),
                /*jshint unused: false*/
                error: function (xhRequest, errorText, thrownError) {
                    alert(errorText);
                },
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

        var dataExisted = $("#divEvent_" + transId).text().length > 0;
        if (!dataExisted) {
            loadMixParlayDetail(transId);
        }

        divEvent.toggle();
    },
    showSystemParlayDetails: function (refNo, custId, betTeam) {
        var url = showSystemParlayDetailsUrl + refNo + "&custId=" + custId;
        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: core.resolveUrl(url),
            /*jshint unused: false*/
            error: function (xhRequest, errorText, thrownError) {
                alert(errorText);
            },
            success: function (response) {
                var height = 550;
                var width = 750;
                var details = '<div><div class="system-parlay-detail-wrapper">' + response + '</div></div>';
                popup.openDialog(details, betTeam, { width: width, height: height });
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
        var url = bettype === 9 || bettype === 29
                ? showBetListResultsUrl + "?refNo=" + refnoMixParlay + "&isSportsbook=false&sportType=" + sporttype
                : showBetListResultsUrl + "?matchId=" + matchid + "&isSportsbook=true&sportType=" + sporttype;

        //popup.openDialogFrame(url, { width: 720, height: 400 });
        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: core.resolveUrl(url),
            /*jshint unused: false*/
            error: function (xhRequest, errorText, thrownError) {
                alert(errorText);
            },
            success: function (response) {
                var height = 400;
                var width = 720;
                var details = '<div style=\"width:700px; height: 380px;overflow:auto;\">' + response + '</div>';
                popup.openDialog(details, language.getString("Results"), { width: width, height: height });
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
