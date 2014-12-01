/*global CORE, LANGUAGE, GRID, COMPONENT, pageContext*/
'use strict';

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
    enableHiddenColumns: false
};

var BET_LIST = BET_LIST || {};
BET_LIST.MEMBER = (function ($this, $) {
    $this.init = function () {
        $('.selectpicker').selectpicker();

        renderDatePicker();
        COMPONENT.registerCheckboxProductEvent();

        // search
        $('#search-form').submit(function (e) {
            reRenderAfterPaged(0, GRID.PageSizes[0]);
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
        reRenderAfterPaged(0, GRID.PageSizes[0]);
    };

    var reRenderAfterPaged = function (currentPage, itemsPerPage) {
        if (gridOptions && gridOptions.pagerOption) {
            currentPage = currentPage >= 0 ? currentPage
                                                : gridOptions.pagerOption.currentPage;
            itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
        }

        COMPONENT.setProductValue(); //Set checked value to input hidden.
        var fromDate = $('#month-picker-control').monPickr("getFromDate", "MM/dd/yyyy");
        var toDate = $('#month-picker-control').monPickr("getToDate", "MM/dd/yyyy");
        var params = {
            CustId: pageContext.CustId,
            FromDate: fromDate,
            ToDate: toDate,
            Status: $("#Status").val() !== 255 ? $.trim($("#Status").val()) : '255',
            Product: $("#Product").val()
        };

        CORE.ajaxJSON({
            url: $('#search-form').attr('action'),
            data: params
        }, function (data) {
            BET_LIST.reRenderAfterPagedSuccessfully(gridOptions, data, true);
        });
    };

    var renderDatePicker = function () {
        var lastdate = new Date(CORE.ServerDate);
        var minDate = new Date(pageContext.MinDate);
        var maxDate = new Date(pageContext.MaxDate);
        maxDate.setFullYear(maxDate.getFullYear() + 3);
        $('#month-picker-control').monPickr({
            plugInId: '#month-picker-control',
            today: CORE.ServerDate,
            maxDate: maxDate,
            minDate: minDate,
            showDateRange: true,
            fromDayId: 'spFrom',
            toDayId: 'spTo',
            fromDayLabel: LANGUAGE.getString('From'),
            toDayLabel: LANGUAGE.getString('To'),
            log: function (msg) {
                console.warn(msg);
            }
        });
        $('#month-picker-control').monPickr("setFromDay", lastdate.getDate());
    };

    $this.showBetListDetails = function (transId, status) {
        fanex.betlist.core.showDetails(transId, status);
    };

    $this.showMixParlayDetail = function (transId, divEvent) {
        var loadMixParlayDetail = function (transId) {
            var url = CORE.resolveUrl('~/BetList/MixParlayDetails', {
                transId: transId
            });
            CORE.ajaxHTML({
                url: url
            }, function (response) {
                divEvent.html(response);
                $(".line:last", divEvent).remove();
                $('.freegrid').fanexGrid('resizeGrid');
            });
        };

        divEvent.toggle();
        var dataExisted = $("#divEvent_" + transId).text().length > 0;
        if (!dataExisted) {
            loadMixParlayDetail(transId);
        }
        else {
            $('.freegrid').fanexGrid('resizeGrid');
        }
    };

    return $this;
} (BET_LIST.MEMBER || {}, jQuery));