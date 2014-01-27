var gridOption = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("Member"), Width: 95, Name: "Member", HeaderAlign: "center", CellAlign: "center", CanResize: true },
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
    gridExpandHeight: 85,
    bodyRowHeight: 29,
    autoResizeColumn: true
};

function OpenIPInfo(ipstring) {
    fanex.totalBets.parlay.getIpInformation(ipstring);
};

function showMP(transId) {
    var divEvent = $('#divEvent_' + transId);
    fanex.totalBets.parlay.showMixParlayDetails(transId, divEvent);
};

function showCombMP(transId, index) {
    var divEvent = $('#divEvent_' + transId + "" + index);
    fanex.totalBets.parlay.showMixParlayDetails(transId, divEvent);
};

var fanex = fanex || {};
fanex.totalBets = fanex.totalBets || {};
fanex.totalBets.parlay = {
    init: function () {

        $('.selectpicker').selectpicker();

        fanex.totalBets.parlay.renderDatePicker();

        // search
        $('#search-form').submit(function (e) {
            fanex.totalBets.parlay.reRenderAfterPaged(0, CORE.pageSizes[0]);
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
        if (gridOption && gridOption.pagerOption) {
            currentPage = currentPage >= 0 ? currentPage
                                                : gridOption.pagerOption.currentPage;
            itemsPerPage = itemsPerPage || gridOption.pagerOption.itemsPerPage;
        }

        var params = {
            BetType: betType
        };

        $.ajax({
            type: 'post',
            data: params,
            beforeSend: function () {
                CORE.blockUI();
            },
            success: function (data) {
                gridOption.pagerOption = {
                    itemsPerPage: CORE.pageSizes[0],
                    itemsPerPageArray: CORE.pageSizes,
                    currentPage: 0,
                    showDetail: true
                };
                gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                $('#BetListGrid').show().fanexGrid(gridOption);

                if (data.IsLastLevel == true)
                    $("#BetListGrid").fanexGrid("hideColumns", gridOptions, [10]);
            },
            complete: function () {
                CORE.unblockUI();
            }
        });
    },
    switchParlayType: function (isMixParlay) {
        if (isMixParlay) {
            $("#mixParlayTab").removeClass('inactive-tab');
            $("#mixParlayTab").addClass('active-tab');
            $("#systemParlayTab").removeClass('active-tab');
            $("#systemParlayTab").addClass('inactive-tab');
            fanex.totalBets.parlay.reRenderAfterPaged(0, CORE.pageSizes[0], 9);
        }
        else {
            $("#mixParlayTab").removeClass('active-tab');
            $("#mixParlayTab").addClass('inactive-tab');
            $("#systemParlayTab").removeClass('inactive-tab');
            $("#systemParlayTab").addClass('active-tab');
            fanex.totalBets.parlay.reRenderAfterPaged(0, CORE.pageSizes[0], 29);
        }
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
                    $('#instruction').html(data);
                    var dialog = POPUP.openDialog($('#instruction'), "IP Information", { modal: false, clickout: true, openid: $(this).attr('id') });
                    $('.ui-tooltip').remove();
                }
            },
            complete: function () {
                CORE.unblockUI();
            }
        });
    },
    renderDatePicker: function () {
        var lastdate = '';
        lastdate = lastMonth + '/' + lastDay + '/' + lastYear;
        $('#month-picker-control').monPickr({
            plugInId: '#month-picker-control',
            today: lastdate,
            maxDate: _today,
            minDate: _minDate,
            showDateRange: true,
            fromDayId: 'spFrom',
            toDayId: 'spTo',
            fromDayLabel: 'From',
            toDayLabel: 'To',
            log: function (msg) {
                console.warn(msg);
            }
        });
        $('#month-picker-control').monPickr("setFromDay", lastDay);
    },
    showParlayDetails: function (transId) {
        var url = "~/Parlay/ParlayDetails?transId=" + transId;
        $.ajax({
            type: "GET",
            contentType: 'application/json',
            dataType: "text",
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
            var url = "~/Parlay/MixParlayDetails?transid=" + transId;
            $.ajax({
                type: "GET",
                contentType: 'application/json',
                dataType: "text",
                cache: false,
                url: CORE.resolveUrl(url),
                error: function (xhRequest, errorText, thrownError) {
                    alert(errorText);
                },
                success: function (response) {
                    divEvent.html(response);
                    $(".line:last", divEvent).remove();
                },
                beforeSend: function () {
                    CORE.blockUI();
                },
                complete: function () {
                    $('#BetListGrid').fanexGrid('resizeGrid');
                    CORE.unblockUI();
                }
            });
        };

        var dataExisted = $("#divEvent_5325904317").text().length > 0;
        if (!dataExisted) {
            loadMixParlayDetail(transId);
        }

        divEvent.toggle();
    },
    showSystemParlayDetails: function (refNo, custId, betTeam) {

        var url = "~/Parlay/SystemParlayDetails?refNo=" + refNo + "&custId=" + custId;
        $.ajax({
            type: "GET",
            contentType: 'application/json',
            dataType: "text",
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
    }
};

$(function () {
    fanex.totalBets.parlay.init();
});