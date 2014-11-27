/*global core, popup, language, moment, pageContext */

var report = report || {};
report.statement = report.statement || {};
report.statement.detail = (function ($) {
    'use strict';
    /* jshint -W101 */
    var agentGridOptions = {
        columns: [
            { HeaderText: language.getString('Date'), Width: 100, Name: "Date", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MM/DD/YYYY", CanResize: false },
            { HeaderText: language.getString('OpeningBalance'), Width: 100, Name: "OpeningBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "Winloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('TransferAmt'), Width: 100, Name: "TransferAmt", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:report.statement.detail.viewTransferDetail({0}, false, {1});'>{2}</a>".format(rowData.AgentId, rowData.Date, cellFormated);
                }
            },
            { HeaderText: language.getString('ClosingBalance'), Width: 100, Name: "ClosingBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        gridExpandHeight: 125,
        showNumberColumn: false,
        showPager: true,
        autoResizeColumn: true
    };

    var memberGridOptions = {
        columns: [
            { HeaderText: language.getString('Date'), Width: 100, Name: "Date", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MM/DD/YYYY", CanResize: false },
            { HeaderText: language.getString('OpeningBalance'), Width: 100, Name: "OpeningBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "Betting", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:report.statement.detail.viewBetList({0}, \"{1}\", \"{2}\");'>{3}</a>".format(rowData.CustId, report.Betting, rowData.Date, cellFormated);
                }
            },
            { HeaderText: language.getString('Outstanding'), Width: 100, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:report.statement.detail.viewBetList({0}, \"{1}\", \"{2}\", true);'>{3}</a>".format(rowData.CustId, report.Outstanding, rowData.Date, cellFormated);
                }
            },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('TransferAmt'), Width: 100, Name: "TransferAmt", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:report.statement.detail.viewTransferDetail({0}, true, {1});'>{2}</a>".format(rowData.CustId, rowData.Date, cellFormated);
                }
            },
            { HeaderText: language.getString('ClosingBalance'), Width: 100, Name: "ClosingBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        gridExpandHeight: 125,
        showNumberColumn: false,
        showPager: true,
        autoResizeColumn: true
    };

    var memberGridOptionsForCash = {
        columns: [
            { HeaderText: language.getString('Date'), Width: 100, Name: "Date", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MM/DD/YYYY", CanResize: false },
            { HeaderText: language.getString('OpeningBalance'), Width: 100, Name: "OpeningBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "Betting", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:report.statement.detail.viewBetList({0}, \"{1}\", \"{2}\");'>{3}</a>".format(rowData.CustId, report.Betting, rowData.Date, cellFormated);
                }
            },
            { HeaderText: language.getString('Outstanding'), Width: 100, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:report.statement.detail.viewBetList({0}, \"{1}\", \"{2}\", true);'>{3}</a>".format(rowData.CustId, report.Outstanding, rowData.Date, cellFormated);
                }
            },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('Deposit'), Width: 100, Name: "Deposit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('Withdrawal'), Width: 100, Name: "Withdrawal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('Bonus'), Width: 100, Name: "Bonus", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('Adjustment'), Width: 100, Name: "Adjustment", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('ClosingBalance'), Width: 100, Name: "ClosingBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        gridExpandHeight: 125,
        showNumberColumn: false,
        showPager: true,
        autoResizeColumn: true
    };
    /* jshint +W101 */

    var genGridRows = function (list, options) {
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

        var rowIndexValue = rowIndex;

        if (options.pagerOption !== undefined) {
            rowIndexValue = options.pagerOption.itemsPerPage * options.pagerOption.currentPage + 1;
        }

        for (var i = 0; i < len; i++) {
            var record = list[i];
            var dataRow = [];

            for (var j = 0; j < columns.length; j++) {
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

                dataRow.push(getGridColumn(j, indexNumberColumn, rowIndex, rowIndexValue, record, columns, isMatchBalance));
            }
            rowIndexValue++;

            bodyRows.push({
                Columns: dataRow,
                CssClass: rowCssClass ? rowCssClass(rowIndex, record) : undefined,
                RowHeight: rowHeight ? rowHeight(rowIndex, record) : undefined
            });

            rowIndex++;
        }
        return bodyRows;
    };

    var getGridColumn = function (colIndex, numberColumn, rowIndex, rowIndexValue, record, columns, isMatchBalance) {
        var rIndex = rowIndex;
        var rowData = record;
        var col = columns[colIndex];
        var colName = col.Name;

        return {
            Name: colName,
            Value: (numberColumn === colIndex) ? rowIndexValue : record[colName],
            CustomCellRender: function (cell, cellFormated) {
                if (col.Formatter) {
                    return col.Formatter(rIndex, rowData, cell, cellFormated);
                } else {
                    return cellFormated;
                }
            },
            TdClass: isMatchBalance ? '' : 'td-highlighted'
        };
    };

    return {
        init: function () {
            $('.datepicker').datepicker({
                endDate: '+3y'
            });
            $('form').submit(function (e) {
                report.statement.detail.submit();
                e.preventDefault();
            });
            $('form').submit();
        },
        submit: function () {
            if (!$.trim($('#Username').val())) {
                alert(language.getString('UsernameRequired'));
                return;
            }
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
                        var gridOptions = data.IsAgent
                            ? agentGridOptions
                            : pageContext.LegendType === 0 ? memberGridOptions : memberGridOptionsForCash;
                        gridOptions.pagerOption = {
                            itemsPerPage: core.pageSizes[2],
                            currentPage: 0,
                            itemsPerPageArray: core.pageSizes,
                            totalItem: data.List.length
                        };

                        gridOptions.bodyRows = genGridRows(data.List, gridOptions);
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
        viewTransferDetail: function (custId, isMember, date) {
            var dateTime = moment(date).format("MM/DD/YYYY");
            var url = core.resolveUrl('~/Report/Statement/TransferDetail', {
                custId: custId,
                fromDate: dateTime,
                toDate: dateTime,
                userName: $('#Username').val(),
                isMember: isMember
            });
            popup.openFrame(url, {
                width: 550,
                height: 400
            });
        },
        viewBetList: function (custId, status, date, hideFooter) {
            var dateTime = moment(date).format("MM/DD/YYYY");
            hideFooter = hideFooter || false;
            var url = core.resolveUrl('~/BetList/WinLoss', {
                custId: custId,
                fromDate: dateTime,
                toDate: dateTime,
                status: status,
                product: $("#Product").val(),
                hideFooter: hideFooter
            });
            popup.openFrame(url, {
                width: 970,
                height: 600
            });
        }
    };
} (jQuery));