var showMixParlayDetailUrl = "~/BetList/MixParlayDetails?transid=";
var showBetListDetailsUrl = "~/BetList/Details?transId=";
var showBetListResultsUrl = "~/BetList/Results";
var showSystemParlayDetailsUrl = "~/BetList/SystemParlayResult?refNo=";

function OpenIPInfo(ipstring) {
    fanex.betlist.core.getIpInformation(ipstring);
};

function showMP(transId) {
    var divEvent = $('#divEvent_' + transId);
    fanex.betlist.core.showMixParlayDetails(transId, divEvent);
};

function showCombMP(transId, index) {
    var divEvent = $('#divEvent_' + transId + "" + index);
    fanex.betlist.core.showMixParlayDetails(transId, divEvent);
};

var fanex = fanex || {};
fanex.betlist = fanex.betlist || {};
fanex.betlist.core = {
    reRenderAfterPagedSuccessfully: function (gridOptions, data, hidedColumn) {
        gridOptions.pagerOption = {
            itemsPerPage: CORE.pageSizes[0],
            itemsPerPageArray: CORE.pageSizes,
            currentPage: 0,
            showDetail: true
        };
        gridOptions.bodyRows = CORE.genGridRows(data.List, gridOptions);
        $('#BetListGrid').show().fanexGrid(gridOptions);

        if (data.IsLastLevel == true)
            $("#BetListGrid").fanexGrid("hideColumns", gridOptions, hidedColumn);
    },
    getIpInformation: function (ipAddress) {
        var urlIpInformation = $("#BetListGrid").data('url-ip');
        $.ajax({ url: urlIpInformation,
            type: 'post',
            data: { IpAddress: ipAddress },
            beforeSend: function () {
                CORE.blockUI();
            },
            success: function (data) {
                if (data != null) {
                    var height = 220;
                    var width = 400;
                    var details = '<div class=\"instruction-content\">' + data + '</div>';
                    POPUP.openDialog(details, LANGUAGE.getString("IpInformation"), {  width: width, height: height, modal: false, clickout: true, openid: $(this).attr('id') });
                    $('.ui-tooltip').remove();
                }
            },
            complete: function () {
                CORE.unblockUI();
            }
        });
    },
    showDetails: function (transId, status) {
        var url = showBetListDetailsUrl + transId + "&status=" + status;
        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: CORE.resolveUrl(url),
            error: function (xhRequest, errorText, thrownError) {
                alert(errorText);
            },
            success: function (response) {
                var height = 280;
                var width = 360;
                var details = '<div>' + response + '</div>';
                var title = LANGUAGE.getString("BetListDetails") + transId;
                POPUP.openDialog(details, title, { width: width, height: height });
            },
            beforeSend: function () {
                CORE.blockUI();
            },
            complete: function () {
                CORE.unblockUI();
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
                url: CORE.resolveUrl(url),
                error: function (xhRequest, errorText, thrownError) {
                    alert(errorText);
                },
                success: function (response) {
                    divEvent.html(response);
                    $(".line:last", divEvent).remove();
                    $('#BetListGrid').fanexGrid('resizeGrid');
                },
                beforeSend: function () {
                    CORE.blockUI();
                },
                complete: function () {
                    CORE.unblockUI();
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
            url: CORE.resolveUrl(url),
            error: function (xhRequest, errorText, thrownError) {
                alert(errorText);
            },
            success: function (response) {
                var height = 550;
                var width = 750;
                var details = '<div><div class="system-parlay-detail-wrapper">' + response + '</div></div>';
                POPUP.openDialog(details, betTeam, { width: width, height: height });
            },
            beforeSend: function () {
                CORE.blockUI();
            },
            complete: function () {
                CORE.unblockUI();
            }
        });
    },
    showResults: function (matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright) {
        var url = bettype == 9 || bettype == 29
                ? showBetListResultsUrl + "?refNo=" + refnoMixParlay + "&isSportsbook=false&sportType=" + sporttype
                : showBetListResultsUrl + "?matchId=" + matchid + "&isSportsbook=true&sportType=" + sporttype;

        //POPUP.openDialogFrame(url, { width: 720, height: 400 });
        $.ajax({
            type: "GET",
            contentType: "text/html",
            cache: false,
            url: CORE.resolveUrl(url),
            error: function (xhRequest, errorText, thrownError) {
                alert(errorText);
            },
            success: function (response) {
                var height = 400;
                var width = 720;
                var details = '<div style=\"width:700px; height: 380px;overflow-y:scroll;\">' + response + '</div>';
                POPUP.openDialog(details, LANGUAGE.getString("Results"), { width: width, height: height });
            },
            beforeSend: function () {
                CORE.blockUI();
            },
            complete: function () {
                CORE.unblockUI();
            }
        });
    }
};

var ViewResult = fanex.betlist.core.showResults;