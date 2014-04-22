/*global core*/

var gridOptions = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: "All", Width: 50, Name: "ActionID", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Action Name", Width: 200, Name: "ActionName", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Input Params", Width: 300, Name: "Command", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Remarks", Width: 300, Name: "Remark", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Re-Invoke", Width: 50, Name: "", HeaderAlign: "center", CellAlign: "center" }
            ],
    showPager: true,
    gridExpandHeight: 100,
    bodyRowHeight: 29
};

var syncmonitoring = (function ($) {
    'use strict';
    return {
        init: function () {
            // search
            $("#actionmonitoring").click(function () {
                syncmonitoring.reRenderAfterPaged(0, core.pageSizes[0]);
            });

        },
        reRenderAfterPaged: function (currentPage, itemsPerPage) {
            if (gridOptions && gridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage
                                                : gridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
            }

            var params = {
                actionID: $("#action").val(),
                taiwanAPIUrl: $("#taiwanapiurl").val()
            };
            
            $.ajax({
                url: "SyncMonitoring",
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    gridOptions.pagerOption = {
                        itemsPerPage: core.pageSizes[0],
                        itemsPerPageArray: core.pageSizes,
                        currentPage: 0,
                        showDetail: true
                    };
                    gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                    $('#monitoringGrid').show().fanexGrid(gridOptions);
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        }
    };
} (jQuery));

$(function () {
    'use strict';
    syncmonitoring.init();
});