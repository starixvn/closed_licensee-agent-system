/*global CORE, LANGUAGE, GRID*/
'use strict';

var REPORT = REPORT || {};
REPORT.STATEMENT = REPORT.STATEMENT || {};
REPORT.STATEMENT.TRANSFER_DETAIL = (function ($this, $) {
    var gridOptions = {
        columns: [
            { HeaderText: LANGUAGE.getString('Date'), Width: 140, Name: "Date", HeaderAlign: "center", CanResize: false,
                CellAlign: "left", FormatDateTime: "MM/DD/YYYY h:mm:ss a"
            },
            { HeaderText: LANGUAGE.getString('Remark'), Width: 240, Name: "TransType", HeaderAlign: "center", CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell) {
                    return ("<div>" + cell.Value + "<br />" + rowData.TransDesc + "</div>");
                }
            },
            { HeaderText: LANGUAGE.getString('Amount'), Width: 100, Name: "TransferAmt", HeaderAlign: "center",
                CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$'
            }
        ],
        showFooter: true,
        autoResizeColumn: true
    };

    var generateFooterRows = function (total) {
        return [
            {
                Columns: [
                    { Value: LANGUAGE.getString("Total"), Name: "Date", CellAlign: "center", MergeColNumber: 2 },
                    { Value: total, Name: "TransferAmt" }
                ]
            }
        ];
    };

    var submit = function () {
        CORE.ajaxJSON({
            url: $('form').attr('action'),
            data: $('form').serialize()
        }, function (response) {
            gridOptions.bodyRows = GRID.generateBodyRows(response.List, gridOptions);
            gridOptions.footerRows = generateFooterRows(response.Total);
            $('#grid').show().fanexGrid(gridOptions);
        });
    };

    $this.init = function () {
        $('form').submit(function (e) {
            submit();
            e.preventDefault();
        });
        $('form').submit();
    };

    return $this;
} (REPORT.STATEMENT.TRANSFER_DETAIL || {}, jQuery));