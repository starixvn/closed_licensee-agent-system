/*global core, popup, language, moment, gridOptionsAgent, gridOptionsMember */

var statementDetailReport = (function ($) {
    'use strict';
    return {
        init: function () {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
            $('#search').click(function () {
                statementDetailReport.dataGridBinding();
                return false;
            });
            statementDetailReport.dataGridBinding();
        },
        dataGridBinding: function () {
            if (!$.trim($('#Username').val())) {
                alert(language.getString('UsernameRequired'));
                return;
            }
            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: core.resolveUrl("~/Report/GetStatementDetailData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        var gridOptions = (data.IsAgent ? gridOptionsAgent : gridOptionsMember);
                        gridOptions.pagerOption = {
                            itemsPerPage: core.pageSizes[2],
                            currentPage: 0,
                            itemsPerPageArray: core.pageSizes,
                            totalItem: data.List.length
                        };

                        gridOptions.bodyRows = statementDetailReport.genGridRows(data.List, gridOptions, $('#IsMember').val());
                        $('#grid').show().fanexGrid(gridOptions);

                        if ($('#spanUsername').length > 0) {
                            $('#spanUsername').html($('#Username').val());
                        }
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
        TransferDetail: function (agentId, isMember, date) {
            var dateTime = moment(date).format("MM/DD/YYYY");
            var url = "Report/TransferDetail?agentId={0}&fromDate={1}&toDate={2}&username={3}&isMember={4}"
                       .format(agentId, dateTime, dateTime, $('#Username').val(), isMember);
            popup.openDialogFrame(url, { width: 465, height: 400 });
        },
        BetList: function (custId, statusIDs, date) {
            var dateTime = moment(date).format("MM/DD/YYYY");
            var url = "BetList/BetListWinlossReport?custId={0}&fromDate={1}&toDate={2}&statusIDs={3}".format(custId, dateTime, dateTime, statusIDs);
            popup.openDialogFrame(url, { width: 970, height: 600 });
        },
        genGridRows: function (list, options) {
            var columns = options.columns;
            var rowCssClass = options.rowCssClass; // custom option
            var rowHeight = options.RowHeight;
            var indexNumberColumn = options.indexNumberColumn; // custom option
            var previousClosingBalance = 0;
            var currentOpeningBalance = 0;

            var bodyRows = [];
            var len = list.length;
            var rowIndex = 0;
            if (typeof value !== 'undefined') {
                var currentPage = options.pagerOption.currentPage || 0;
                rowIndex = currentPage * options.pagerOption.itemsPerPage;
            }

            for (var i = 0; i < len; i++) {
                var record = list[i];
                var dataRow = [];

                for (var j = 0; j < columns.length; j++) {
                    var rIndex = rowIndex;
                    var col = columns[j];
                    var colName = col.Name;
                    var isMatchBalance = true;

                    if (colName === 'ClosingBalance') {
                        previousClosingBalance = record[colName];
                    }

                    if (colName === 'OpeningBalance') {
                        currentOpeningBalance = record[colName];

                        if (i !== 0 && previousClosingBalance !== currentOpeningBalance) {
                            isMatchBalance = false;
                        }
                    }

                    dataRow.push({
                        Name: colName,
                        Value: (indexNumberColumn === j) ? rIndex + 1 : record[colName],
                        TdClass: isMatchBalance ? '' : 'td-highlighted'
                    });
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
    };
} (jQuery));

$(function () {
    'use strict';
    statementDetailReport.init();
});