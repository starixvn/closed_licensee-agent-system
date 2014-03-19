/*global CORE, LANGUAGE*/

var gridOptions = {
    /*jshint strict:false */
    columns: [
                { HeaderText: LANGUAGE.getString('Date'), Width: 150, Name: "Date", HeaderAlign: "center", 
                  CellAlign: "left", FormatDateTime: "MM/DD/YYYY h:mm:ss a" },
                { HeaderText: LANGUAGE.getString('Remark'), Width: 150, Name: "TransType", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true,
                    Formatter: function (rowIndex, rowData, cell) {
                        return ("<div>" + cell.Value + "<br />" + rowData.TransDesc + "</div>");
                    }
                },
                { HeaderText: LANGUAGE.getString('Amount'), Width: 100, Name: "TransferAmt", HeaderAlign: "center", 
                  CellAlign: "right", CanResize: true, FormatNumber: "0,0.00", FormatNegative: '$' }
            ],
    gridExpandHeight: 20,
    bodyRowHeight: 26,
    showFooter: true,
    autoResizeColumn: true
};

var Transfer = (function ($) {
    /*jshint strict:false */
    return {
        init: function () {
            Transfer.dataGridBinding();
        },
        dataGridBinding: function () {
            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: CORE.resolveUrl("~/Report/GetTransferData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        gridOptions.bodyRows = CORE.genGridRows(data.List, gridOptions);
                        gridOptions.footerRows = Transfer.footerRow(data.Total);
                        $('#grid').show().fanexGrid(gridOptions);
                        CORE.unblockUI();
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        footerRow: function (total) {
            var footerRows = [
                                {
                                    Columns: [
                                        { Value: LANGUAGE.getString("Total"), Name: "Date", CellAlign: "center", MergeColNumber: 2 },
                                        { Value: total, Name: "TransferAmt" }
                                    ]
                                }
                        ];
            return footerRows;
        }
    };
} (jQuery));

$(function () {
    /*jshint strict:false */
    Transfer.init();
});