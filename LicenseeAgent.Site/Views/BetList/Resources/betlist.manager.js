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

var showMixParlayDetailUrl = "~/BetList/MixParlayDetails?transid=";
var showBetListDetailsUrl = "~/BetList/Details?transId=";
var showBetListResultsUrl = "~/BetList/Results";
var showSystemParlayDetailsUrl = "~/BetList/SystemParlayResult?refNo=";

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
                StatusIDs: $("#Status").val() != 255 ? $.trim($("#Status").val()) : 'All'
            };

            $.ajax({
                url: urlGetBetList,
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    fanex.betlist.core.reRenderAfterPagedSuccessfully(data, [9]);
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
            fanex.betlist.core.showDetails(transId, status);
        },
        showMixParlayDetail: function (transId, divEvent) {
            var loadMixParlayDetail = function (transId) {
                var url = showMixParlayDetailurl + transId;
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

            var dataExisted = $("#divEvent_" + transId).text().length > 0;
            if (!dataExisted) {
                loadMixParlayDetail(transId);
            }

            divEvent.toggle();
        },
        showSystemParlayResult: function (refNo, custId, betTeam) {
            fanex.betlist.core.showSystemParlayDetails(refNo, custId, betTeam);
        }
    }
} (jQuery);

$(function () {
    BETLIST.init();
});