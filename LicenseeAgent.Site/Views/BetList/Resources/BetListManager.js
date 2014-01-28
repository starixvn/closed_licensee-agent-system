var gridOptions = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("TransTime"), Width: 95, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
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
    gridExpandHeight: 100,
    bodyRowHeight: 29
};

function OpenIPInfo(ipstring) {
    BETLIST.getIpInformation(ipstring);
};

function showMP(transId) {
    var divEvent = $('#divEvent_' + transId);
    BETLIST.showMixParlayDetail(transId, divEvent);
};

function showCombMP(transId, index) {
    var divEvent = $('#divEvent_' + transId + "" + index);
    BETLIST.showMixParlayDetail(transId, divEvent);
};

var BETLIST = function ($) {
    return {
        init: function () {

            $('.selectpicker').selectpicker();

            BETLIST.renderDatePicker();

            // search
            $('#search-form').submit(function (e) {
                BETLIST.reRenderAfterPaged(0, CORE.pageSizes[0]);
                e.preventDefault();
            });

            // get bet list
            BETLIST.reRenderAfterPaged(0, CORE.pageSizes[0]);
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage) {
            if (gridOptions && gridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage
                                                : gridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
            }

            var urlGetBetList = $('#search-form').attr('action');
            var fromDate = $('#month-picker-control').monPickr("getFromDate", "MM/dd/yyyy");
            var toDate = $('#month-picker-control').monPickr("getToDate", "MM/dd/yyyy");
            var params = {
                CustId: custId,
                FromDate: fromDate,
                ToDate: toDate,
                StatusId: $("#Status").val() != 255 ? $.trim($("#Status").val()) : ''
            };

            $.ajax({
                url: urlGetBetList,
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    gridOptions.pagerOption = {
                        itemsPerPage: CORE.pageSizes[0],
                        itemsPerPageArray: CORE.pageSizes,
                        currentPage: 0,
                        showDetail: true
                    };
                    gridOptions.bodyRows = CORE.genGridRows(data.List, gridOptions);
                    $('#BetListGrid').show().fanexGrid(gridOptions);

                    if(data.IsLastLevel == true)
                        $("#BetListGrid").fanexGrid("hideColumns", gridOptions, [9]);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
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
        showBetListDetails: function (transId, status) {
            var url = "~/BetList/Details?transId=" + transId + "&status=" + status;
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
        showMixParlayDetail: function (transId, divEvent) {
            var loadMixParlayDetail = function (transId) {
                var url = "~/BetList/MixParlayDetails?transid=" + transId;
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

            var dataExisted = $("#divEvent_5325904317").text().length > 0;
            if (!dataExisted) {
                loadMixParlayDetail(transId);
            }

            divEvent.toggle();
        },
        showSystemParlayResult: function (refNo, custId, betTeam) {

            var url = "~/BetList/SystemParlayResult?refNo=" + refNo + "&custId=" + custId;
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

    }
} (jQuery);

$(function () {
    BETLIST.init();
});