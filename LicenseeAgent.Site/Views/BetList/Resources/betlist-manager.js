/*global core, language, showMixParlayDetailurl, pageContext, report*/

var gridOptions = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: language.getString("TransTime"), Width: 90, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: language.getString("Choice"), Width: 335, Name: "Choice", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: language.getString("AgentPT"), Width: 75, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
            ],
    showPager: true,
    enableHiddenColumns: false,
    gridExpandHeight: 200,
    bodyRowHeight: 29
};

var betList = (function ($) {
    'use strict';
    return {
        init: function () {
            $('.selectpicker').selectpicker();

            betList.renderDatePicker();
            report.registerCheckboxProductEvent();

            // search
            $('#search-form').submit(function (e) {
                betList.reRenderAfterPaged(0, core.pageSizes[0]);
                e.preventDefault();
            });

            var hiddenColumns = [];
            if (pageContext.LegendType === 1) {
                hiddenColumns.push(8);
            }
            if (pageContext.IsLastLevel) {
                hiddenColumns.push(9);
            }
            gridOptions.hiddenColumns = hiddenColumns;

            // get bet list
            betList.reRenderAfterPaged(0, core.pageSizes[0]);
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage) {
            if (gridOptions && gridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage
                                                : gridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
            }

            report.setProductValue(); //Set checked value to input hidden.
            var urlGetBetList = $('#search-form').attr('action');
            var fromDate = $('#month-picker-control').monPickr("getFromDate", "MM/dd/yyyy");
            var toDate = $('#month-picker-control').monPickr("getToDate", "MM/dd/yyyy");

            var params = {
                CustId: pageContext.CustId,
                FromDate: fromDate,
                ToDate: toDate,
                StatusIDs: $("#Status").val() !== 255 ? $.trim($("#Status").val()) : '255',
                ProductIds: $("#Product").val()
            };

            $.ajax({
                url: urlGetBetList,
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== undefined && data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                        return;
                    }
                    fanex.betlist.core.reRenderAfterPagedSuccessfully(gridOptions, data, true);
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        renderDatePicker: function () {
            var lastdate = new Date(core.ServerDate);
            $('#month-picker-control').monPickr({
                plugInId: '#month-picker-control',
                today: core.ServerDate,
                maxDate: new Date(pageContext.MaxDate),
                minDate: new Date(pageContext.MinDate),
                showDateRange: true,
                fromDayId: 'spFrom',
                toDayId: 'spTo',
                fromDayLabel: 'From',
                toDayLabel: 'To',
                log: function (msg) {
                    console.warn(msg);
                }
            });
            $('#month-picker-control').monPickr("setFromDay", lastdate.getDate());
        },
        showBetListDetails: function (transId, status) {
            fanex.betlist.core.showDetails(transId, status);
        },
        showMixParlayDetail: function (transId, divEvent) {
            var loadMixParlayDetail = function (transId) {
                var url = showMixParlayDetailurl + transId;
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
                loadMixParlayDetail(transId);
            }
            else {
                $('#BetListGrid').fanexGrid('resizeGrid');
            }
        }
    };
} (jQuery));

$(function () {
    'use strict';
    betList.init();
});