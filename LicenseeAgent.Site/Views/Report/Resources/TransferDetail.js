var gridOption = {
    columns: [
                { HeaderText: LANGUAGE.getString('Date'), Width: 150, Name: "Date", HeaderAlign: "center", CellAlign: "left", FormatDateTime: "MM/DD/YYYY h:mm:ss a" },
                { HeaderText: LANGUAGE.getString('Remark'), Width: 150, Name: "TransType", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true,
                    Formatter: function (rowIndex, rowData, cell, cellFormated) {
                        return ("<div>" + cell.Value + "<br />" + rowData.TransDesc + "</div>");
                    }
                },
                { HeaderText: LANGUAGE.getString('Amount'), Width: 100, Name: "TransferAmt", HeaderAlign: "center", CellAlign: "right", CanResize: true, FormatNumber: "0,0.00", FormatNegative: '$' }
            ],
    gridExpandHeight: 20,
    bodyRowHeight: 26,
    showFooter: true,
    autoResizeColumn: true
};

var TRANSFER = function ($) {
    return {
        init: function () {
            TRANSFER.dataGridBinding();
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
                    if (data.ErrCode == 0) {
                        gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                        gridOption.footerRows = TRANSFER.footerRow(data.Total);
                        $('#grid').show().fanexGrid(gridOption);
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
    }
} (jQuery);

$(function () {
    TRANSFER.init();
});