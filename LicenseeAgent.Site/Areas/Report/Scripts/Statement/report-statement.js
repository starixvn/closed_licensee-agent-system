/*global CORE, LANGUAGE, POPUP, GRID, pageContext*/
'use strict';

var REPORT = REPORT || {};
REPORT.STATEMENT = (function ($this, $) {
    /* jshint -W101 */
    var agentGridOptions = {
        columns: [
            { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Username'), Width: 150, Name: "Username", AllowTruncate: true, CanResize: false, HeaderAlign: "center", CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return ("<a href='javascript:REPORT.STATEMENT.viewDetail({0}, \"{1}\");'>" + cellFormated + "</a>").format(rowData.AgentId, cell.Value);
                }
            },
            { HeaderText: LANGUAGE.getString('FullName'), Width: 150, Name: "FullName", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true, CanResize: false,
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return $.trim(cell.Value) === '' ? '--- ---' : cellFormated;
                }
            },
            { HeaderText: LANGUAGE.getString('OpeningBalance'), Width: 100, Name: "OpeningBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "Winloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('TransferAmt'), Width: 100, Name: "TransferAmt", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return ("<a class='num-link' href='javascript:REPORT.STATEMENT.viewTransferDetail({0}, \"{1}\");'>" + cellFormated + "</a>").format(rowData.AgentId, rowData.Username);
                }
            },
            { HeaderText: LANGUAGE.getString('ClosingBalance'), Width: 100, Name: "ClosingBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        showNumberColumn: true,
        showPager: true,
        autoResizeColumn: true
    };

    var memberGridOptions = {
        columns: [
            { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Username'), Width: 150, Name: "Username", HeaderAlign: "center", AllowTruncate: true, CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell) {
                    return ("<a href='javascript:REPORT.STATEMENT.viewDetail({0}, \"{1}\");'>" + cell.Value + "</a>").format(rowData.CustId, cell.Value);
                }
            },
            { HeaderText: LANGUAGE.getString('FullName'), Width: 150, Name: "LastName", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true,
                Formatter: function (rowIndex, rowData) {
                    return ((rowData.LastName === '' ? '--- ' : rowData.LastName) + (rowData.FirstName === '' ? '---' : rowData.FirstName));
                }
            },
            { HeaderText: LANGUAGE.getString('OpeningBalance'), Width: 100, Name: "OpeningBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "Betting", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Outstanding'), Width: 100, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('TransferAmt'), Width: 100, Name: "TransferAmt", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return ("<a class='num-link' href='javascript:REPORT.STATEMENT.viewTransferDetail({0}, \"{1}\");'>" + cellFormated + "</a>").format(rowData.CustId, rowData.Username);
                }
            },
            { HeaderText: LANGUAGE.getString('ClosingBalance'), Width: 100, Name: "ClosingBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        showNumberColumn: true,
        showPager: true,
        autoResizeColumn: true
    };
    /* jshint +W101 */

    var submit = function () {
        CORE.ajaxJSON({
            url: $('form').attr('action'),
            data: $('form').serialize()

        }, function (response) {
            var gridOptions = pageContext.IsLastLevel ? memberGridOptions : agentGridOptions;
            gridOptions.pagerOption = {
                itemsPerPage: GRID.PageSizes[2],
                currentPage: 0,
                itemsPerPageArray: GRID.PageSizes,
                totalItem: response.List.length
            };

            gridOptions.bodyRows = GRID.generateBodyRows(response.List, gridOptions);
            $('#grid').show().fanexGrid(gridOptions);
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

    $this.viewDetail = function (agentId, userName) {
        var url = CORE.resolveUrl('~/Report/Statement/Detail', {
            agentId: agentId,
            fromDate: $('#FromDate').val(),
            toDate: $('#ToDate').val(),
            userName: userName,
            isMember: pageContext.IsLastLevel,
            parentUsername: $('#Username').val()
        });
        location.href = url;
    };

    $this.viewTransferDetail = function (custId, userName) {
        var url = CORE.resolveUrl('~/Report/Statement/TransferDetail', {
            custId: custId,
            fromDate: $('#FromDate').val(),
            toDate: $('#ToDate').val(),
            userName: userName,
            isMember: pageContext.IsLastLevel
        });
        POPUP.openFrame(url, {
            width: 550,
            height: 400
        });
    };

    return $this;
} (REPORT.STATEMENT || {}, jQuery));