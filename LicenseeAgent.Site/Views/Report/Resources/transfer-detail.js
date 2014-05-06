/*global core, language*/

var transferDetail = (function ($) {
    'use strict';

    var gridOptions = {
        columns: [
                { HeaderText: language.getString('Date'), Width: 140, Name: "Date", HeaderAlign: "center",
                    CellAlign: "left", FormatDateTime: "MM/DD/YYYY h:mm:ss a"
                },
                { HeaderText: language.getString('Remark'), Width: 160, Name: "TransType", HeaderAlign: "center", CellAlign: "left", CanResize: true,
                    Formatter: function (rowIndex, rowData, cell) {
                        return ("<div>" + cell.Value + "<br />" + rowData.TransDesc + "</div>");
                    }
                },
                { HeaderText: language.getString('Amount'), Width: 100, Name: "TransferAmt", HeaderAlign: "center",
                    CellAlign: "right", CanResize: true, FormatNumber: "0,0.00", FormatNegative: '$'
                }
            ],
        gridExpandHeight: 20,
        bodyRowHeight: 26,
        showFooter: true,
        autoResizeColumn: true
    };

    return {
        init: function () {
            transferDetail.dataGridBinding();
        },
        dataGridBinding: function () {
            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: core.resolveUrl("~/Report/GetTransferData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                        gridOptions.footerRows = transferDetail.footerRow(data.Total);
                        $('#grid').show().fanexGrid(gridOptions);
                        core.unblockUI();
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        footerRow: function (total) {
            var footerRows = [
                                {
                                    Columns: [
                                        { Value: language.getString("Total"), Name: "Date", CellAlign: "center", MergeColNumber: 2 },
                                        { Value: total, Name: "TransferAmt" }
                                    ]
                                }
                        ];
            return footerRows;
        }
    };
} (jQuery));

$(function () {
    'use strict';
    transferDetail.init();
});