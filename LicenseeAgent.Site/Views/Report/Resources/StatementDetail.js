var STATEMENTDETAIL = function ($) {
    return {
        init: function () {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
            $('#search').click(function () {
                STATEMENTDETAIL.dataGridBinding();
                return false;
            });
            STATEMENTDETAIL.dataGridBinding();
        },
        dataGridBinding: function () {            
            if (!$.trim($('#Username').val())) {
                alert(LANGUAGE.getString('UsernameRequired'));
                return;
            }
            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: CORE.resolveUrl("~/Report/GetStatementDetailData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        gridOptions.pagerOption = {
                            itemsPerPage: CORE.pageSizes[2],
                            currentPage: 0,
                            itemsPerPageArray: CORE.pageSizes,
                            totalItem: data.List.length
                        };

                        gridOptions.bodyRows = STATEMENTDETAIL.genGridRows(data.List, gridOptions, $('#IsMember').val());
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
        TransferDetail: function (agentId, isMember, date) {
            var dateTime = moment(date).format("MM/DD/YYYY");
            var url = "Report/TransferDetail?agentId={0}&fromDate={1}&toDate={2}&username={3}&isMember={4}".format(agentId, dateTime, dateTime, $('#Username').val(), isMember);
            POPUP.openDialogFrame(url, { width: 465, height: 400 });
        },
        BetList: function (custId, statusIDs, date) {
            var dateTime = moment(date).format("MM/DD/YYYY");
            var url = "BetList/BetListWinlossReport?custId={0}&fromDate={1}&toDate={2}&statusIDs={3}".format(custId, dateTime, dateTime, statusIDs);
            POPUP.openDialogFrame(url, { width: 970, height: 600 });
        },
        genGridRows: function (list, options) {
            var columns = options.columns;
            var rowCssClass = options.rowCssClass; // custom option
            var tdCssClass = options.tdCssClass;
            var rowHeight = options.RowHeight;
            var indexNumberColumn = options.indexNumberColumn; // custom option
            var previousClosingBalance = 0;
            var currentOpeningBalance = 0;
            var highlightCss = 'td-highlighted';

            var bodyRows = [];
            var len = list.length;
            var rowIndex = 0;
            if (typeof value != 'undefined') {
                var currentPage = options.pagerOption.currentPage || 0;
                rowIndex = currentPage * options.pagerOption.itemsPerPage;
            }

            for (var i = 0; i < len; i++) {
                var record = list[i];
                var dataRow = [];

                for (var j = 0; j < columns.length; j++) {
                    (function () {
                        var rIndex = rowIndex;
                        var rowData = record;
                        var col = columns[j];
                        var colName = col.Name;
                        var isMatchBalance = true;

                        if (colName == 'ClosingBalance') {
                            previousClosingBalance = record[colName];
                        }

                        if (colName == 'OpeningBalance') {
                            currentOpeningBalance = record[colName];

                            if (i != 0 && previousClosingBalance != currentOpeningBalance) {
                                isMatchBalance = false;
                            }
                        }

                        dataRow.push({
                            Name: colName,
                            Value: (indexNumberColumn == j) ? rIndex + 1 : record[colName],
                            CustomCellRender: function (cell, cellFormated) {
                                if (col.Formatter) {
                                    return col.Formatter(rIndex, rowData, cell, cellFormated);
                                } else
                                    return cellFormated;
                            },
                            TdClass: isMatchBalance ? '' : 'td-highlighted'
                        });
                    } ());
                }

                bodyRows.push({
                    Columns: dataRow,
                    CssClass: rowCssClass ? rowCssClass(rowIndex, record) : undefined,
                    RowHeight: rowHeight ? rowHeight(rowIndex, record) : undefined
                });

                rowIndex++;
            }
            return bodyRows;
        }
    }
} (jQuery);

$(function () {
    STATEMENTDETAIL.init();
});