/*global CORE, LANGUAGE, POPUP, ENUMS, GRID, moment, pageContext */
'use strict';

var REPORT = REPORT || {};
REPORT.STATEMENT = REPORT.STATEMENT || {};
REPORT.STATEMENT.DETAIL = (function ($this, $) {
    /* jshint -W101 */
    var agentGridOptions = {
        columns: [
            { HeaderText: LANGUAGE.getString('Date'), Width: 100, Name: "Date", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MM/DD/YYYY", CanResize: false },
            { HeaderText: LANGUAGE.getString('OpeningBalance'), Width: 100, Name: "OpeningBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "Winloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('TransferAmt'), Width: 100, Name: "TransferAmt", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:REPORT.STATEMENT.DETAIL.viewTransferDetail({0}, false, {1});'>{2}</a>".format(rowData.AgentId, rowData.Date, cellFormated);
                }
            },
            { HeaderText: LANGUAGE.getString('ClosingBalance'), Width: 100, Name: "ClosingBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        showNumberColumn: false,
        showPager: true,
        autoResizeColumn: true
    };

    var memberGridOptions = {
        columns: [
            { HeaderText: LANGUAGE.getString('Date'), Width: 100, Name: "Date", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MM/DD/YYYY", CanResize: false },
            { HeaderText: LANGUAGE.getString('OpeningBalance'), Width: 100, Name: "OpeningBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "Betting", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:REPORT.STATEMENT.DETAIL.viewBetList({0}, \"{1}\", \"{2}\");'>{3}</a>".format(rowData.CustId, ENUMS.TicketStatus.Betting, rowData.Date, cellFormated);
                }
            },
            { HeaderText: LANGUAGE.getString('Outstanding'), Width: 100, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:REPORT.STATEMENT.DETAIL.viewBetList({0}, \"{1}\", \"{2}\", true);'>{3}</a>".format(rowData.CustId, ENUMS.TicketStatus.Outstanding, rowData.Date, cellFormated);
                }
            },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('TransferAmt'), Width: 100, Name: "TransferAmt", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:REPORT.STATEMENT.DETAIL.viewTransferDetail({0}, true, {1});'>{2}</a>".format(rowData.CustId, rowData.Date, cellFormated);
                }
            },
            { HeaderText: LANGUAGE.getString('ClosingBalance'), Width: 100, Name: "ClosingBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        showNumberColumn: false,
        showPager: true,
        autoResizeColumn: true
    };

    var memberGridOptionsForCash = {
        columns: [
            { HeaderText: LANGUAGE.getString('Date'), Width: 100, Name: "Date", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MM/DD/YYYY", CanResize: false },
            { HeaderText: LANGUAGE.getString('OpeningBalance'), Width: 100, Name: "OpeningBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "Betting", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:REPORT.STATEMENT.DETAIL.viewBetList({0}, \"{1}\", \"{2}\");'>{3}</a>".format(rowData.CustId, ENUMS.TicketStatus.Betting, rowData.Date, cellFormated);
                }
            },
            { HeaderText: LANGUAGE.getString('Outstanding'), Width: 100, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return "<a class='num-link' href='javascript:REPORT.STATEMENT.DETAIL.viewBetList({0}, \"{1}\", \"{2}\", true);'>{3}</a>".format(rowData.CustId, ENUMS.TicketStatus.Outstanding, rowData.Date, cellFormated);
                }
            },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('Deposit'), Width: 100, Name: "Deposit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('Withdrawal'), Width: 100, Name: "Withdrawal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('Bonus'), Width: 100, Name: "Bonus", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('Adjustment'), Width: 100, Name: "Adjustment", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('ClosingBalance'), Width: 100, Name: "ClosingBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        showNumberColumn: false,
        showPager: true,
        autoResizeColumn: true
    };
    /* jshint +W101 */

    var generateBodyRows = function (list, options) {
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

    var submit = function () {
        if (!$.trim($('#Username').val())) {
            alert(LANGUAGE.getString('UsernameRequired'));
            return;
        }
        CORE.ajaxJSON({
            url: $('form').attr('action'),
            data: $('form').serialize()
        }, function (response) {
            var gridOptions = response.IsAgent
                ? agentGridOptions
                : pageContext.LegendType === 0 ? memberGridOptions : memberGridOptionsForCash;
            gridOptions.pagerOption = {
                itemsPerPage: GRID.PageSizes[2],
                currentPage: 0,
                itemsPerPageArray: GRID.PageSizes,
                totalItem: response.List.length
            };

            gridOptions.bodyRows = generateBodyRows(response.List, gridOptions);
            $('#grid').show().fanexGrid(gridOptions);

            if ($('#spanUsername').length > 0) {
                $('#spanUsername').html($('#Username').val());
            }
        });
    };

    $this.init = function () {
        $('.datepicker').datepicker({
            endDate: '+3y'
        });
        $('form').submit(function (e) {
            submit();
            e.preventDefault();
        });
        $('form').submit();
    };

    $this.viewTransferDetail = function (custId, isMember, date) {
        var dateTime = moment(date).format("MM/DD/YYYY");
        var url = CORE.resolveUrl('~/Report/Statement/TransferDetail', {
            custId: custId,
            fromDate: dateTime,
            toDate: dateTime,
            userName: $('#Username').val(),
            isMember: isMember
        });
        POPUP.openFrame(url, {
            width: 550,
            height: 400
        });
    };

    $this.viewBetList = function (custId, status, date, hideFooter) {
        var dateTime = moment(date).format("MM/DD/YYYY");
        hideFooter = hideFooter || false;
        var url = CORE.resolveUrl('~/BetList/WinLoss', {
            custId: custId,
            fromDate: dateTime,
            toDate: dateTime,
            status: status,
            product: $("#Product").val(),
            hideFooter: hideFooter
        });
        POPUP.openFrame(url, {
            width: 970,
            height: 600
        });
    };

    return $this;
} (REPORT.STATEMENT.DETAIL || {}, jQuery));