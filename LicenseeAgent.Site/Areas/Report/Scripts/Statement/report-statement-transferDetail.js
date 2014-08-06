/*global core, language*/

var report = report || {};
report.statement = report.statement || {};
report.statement.transferDetail = (function ($) {
    'use strict';
    var gridOptions = {
        columns: [
                { HeaderText: language.getString('Date'), Width: 140, Name: "Date", HeaderAlign: "center", CanResize: false,
                    CellAlign: "left", FormatDateTime: "MM/DD/YYYY h:mm:ss a"
                },
                { HeaderText: language.getString('Remark'), Width: 240, Name: "TransType", HeaderAlign: "center", CellAlign: "left",
                    Formatter: function (rowIndex, rowData, cell) {
                        return ("<div>" + cell.Value + "<br />" + rowData.TransDesc + "</div>");
                    }
                },
                { HeaderText: language.getString('Amount'), Width: 100, Name: "TransferAmt", HeaderAlign: "center",
                    CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$'
                }
            ],
        gridExpandHeight: 20,
        bodyRowHeight: 26,
        showFooter: true,
        autoResizeColumn: true
    };

    var genFooterRow = function (total) {
        return [
            {
                Columns: [
                    { Value: language.getString("Total"), Name: "Date", CellAlign: "center", MergeColNumber: 2 },
                    { Value: total, Name: "TransferAmt" }
                ]
            }
        ];
    };

    return {
        init: function () {
            $('form').submit(function (e) {
                report.statement.transferDetail.submit();
                e.preventDefault();
            });
            $('form').submit();
        },
        submit: function () {
            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: $('form').attr('action'),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                        gridOptions.footerRows = genFooterRow(data.Total);
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
        }
    };
} (jQuery));