var gridOptions = {
    columns: [
                { HeaderText: LANGUAGE.getString("Agent"), Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter:
                    function (rowIndex, rowData, cell, cellFormated) {
                        return '<a href="#" onclick="OutstandingDrilldown(' + rowData.Id + ')">' + rowData.Username + '</a>';
                    } 
                },
                { HeaderText: LANGUAGE.getString("Outstanding"), Width: 200, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '($)' },
                { HeaderText: LANGUAGE.getString("AgentShare"), Width: 200, Name: "Positiontaking", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '($)' }
            ],
    showPager: true,
    showFooter: true,
    gridExpandHeight: 100,
    bodyRowHeight: 29
};

var outstandingReportUrl = "Outstanding";
function OutstandingDrilldown(agentId) {
    OUTSTANDING.reRenderAfterPaged(0, CORE.pageSizes[0], agentId);
}

var OUTSTANDING = function ($) {
    return {
        init: function () {
            OUTSTANDING.reRenderAfterPaged(0, CORE.pageSizes[0], -1);
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage, agentId) {
            if (gridOptions && gridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage
                                                : gridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
            }

            var params = {
                AgentId: agentId
            };

            $.ajax({
                url: outstandingReportUrl,
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    var footerRows = [
                            {
                                Columns: [
                                    { Value: "Total", Name: "Username", CellAlign: "left" },
                                    { Value: data.SumOutstanding, Name: "Outstanding" },
                                    { Value: data.SumPositiontaking, Name: "Positiontaking" }
                                ]
                            }
                    ];

                    gridOptions.pagerOption = {
                        itemsPerPage: CORE.pageSizes[0],
                        itemsPerPageArray: CORE.pageSizes,
                        currentPage: 0,
                        showDetail: true
                    };
                    gridOptions.bodyRows = CORE.genGridRows(data.List, gridOptions);
                    gridOptions.footerRows = footerRows;
                    if (data.IsLastLevel == true) {
                        gridOptions.columns[0].HeaderText = LANGUAGE.getString("Member");
                    }
                    $('#BetListGrid').show().fanexGrid(gridOptions);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        }
    }
} (jQuery);

$(function () {
    OUTSTANDING.init();
});