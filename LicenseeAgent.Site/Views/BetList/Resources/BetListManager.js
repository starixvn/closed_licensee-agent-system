var gridOption = {
    columns: [
                { HeaderText: "No.", Width: 30, Name: "Index", CellAlign: "center" },
                { HeaderText: "TransTime", Width: 100, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Choice", Width: 350, Name: "Choice", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Odds", Width: 40, Name: "Odds", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Stake", Width: 60, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Winloss", Width: 60, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Status", Width: 80, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "AgentPT", Width: 80, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "AgentComm", Width: 80, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Feature", Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
            ],
    showPager: true,
    customPager: true,
    gridExpandHeight: 100,
    bodyRowHeight: 29,
    pagerOption: {
        itemsPerPageArray: [10, 20, 50, 100, 500]
    }
};

function OpenIPInfo(ipstring)
{
    BETLIST.getIpInformation(ipstring);
};

var BETLIST = function ($) {
    return {
        init: function () {
            BETLIST.renderDatePicker();

            // search
            $('#search-form').submit(function (e) {
                BETLIST.reRenderAfterPaged(0, 10);
                e.preventDefault();
            });

            // get bet list
            //BETLIST.reRenderAfterPaged(0, CORE.pageSizes[0]);
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage) {
            if (gridOption && gridOption.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage
                                                : gridOption.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOption.pagerOption.itemsPerPage;
            }

            var urlGetBetList = $('#search-form').attr('action');
            var fromDate = $('#month-picker-control').monPickr("getFromDate", "MM/dd/yyyy");
            var toDate = $('#month-picker-control').monPickr("getToDate", "MM/dd/yyyy");
            var params = {
                FromDate: fromDate,
                ToDate: toDate,
                StatusId: $("#Status").val() != -1 ? $.trim($("#Status").val()) : '',
                PageIndex: currentPage,
                PageSize: itemsPerPage
            };

            $.ajax({
                url: urlGetBetList,
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: CORE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: BETLIST.reRenderAfterPaged
                    };
                    gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                    $('#BetListGrid').show().fanexGrid(gridOption);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        getIpInformation: function (ipAddress) {
            $.ajax({ url: "Customer/GetIpInformation",
                type: 'post',
                data: { IpAddress: ipAddress },
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data != null) {
                        $('#instruction').html(data);
                        var dialog = POPUP.openDialog($('#instruction'), msg.IpInformation, { modal: false, clickout: true, openid: $(this).attr('id') });
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
        }
    }
} (jQuery);

$(function () {
    BETLIST.init();

});