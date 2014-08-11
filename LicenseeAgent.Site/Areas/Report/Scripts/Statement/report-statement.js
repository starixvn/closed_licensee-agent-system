/*global core, popup, language, pageContext*/

var report = report || {};
report.statement = (function ($) {
    'use strict';
    /* jshint -W101 */
    var agentGridOptions = {
        columns: [
            { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString('Username'), Width: 150, Name: "Username", AllowTruncate: true, CanResize: false, HeaderAlign: "center", CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return ("<a href='javascript:report.statement.viewDetail({0}, \"{1}\");'>" + cellFormated + "</a>").format(rowData.AgentId, cell.Value);
                }
            },
            { HeaderText: language.getString('FullName'), Width: 150, Name: "FullName", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true, CanResize: false,
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return $.trim(cell.Value) === '' ? '--- ---' : cellFormated;
                }
            },
            { HeaderText: language.getString('OpeningBalance'), Width: 100, Name: "OpeningBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "Winloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('TransferAmt'), Width: 100, Name: "TransferAmt", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return ("<a class='num-link' href='javascript:report.statement.viewTransferDetail({0}, \"{1}\");'>" + cellFormated + "</a>").format(rowData.AgentId, rowData.Username);
                }
            },
            { HeaderText: language.getString('ClosingBalance'), Width: 100, Name: "ClosingBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        gridExpandHeight: 125,
        showNumberColumn: true,
        showPager: true,
        autoResizeColumn: true
    };

    var memberGridOptions = {
        columns: [
            { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString('Username'), Width: 150, Name: "Username", HeaderAlign: "center", AllowTruncate: true, CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell) {
                    return ("<a href='javascript:report.statement.viewDetail({0}, \"{1}\");'>" + cell.Value + "</a>").format(rowData.CustId, cell.Value);
                }
            },
            { HeaderText: language.getString('FullName'), Width: 150, Name: "LastName", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true,
                Formatter: function (rowIndex, rowData) {
                    return ((rowData.LastName === '' ? '--- ' : rowData.LastName) + (rowData.FirstName === '' ? '---' : rowData.FirstName));
                }
            },
            { HeaderText: language.getString('OpeningBalance'), Width: 100, Name: "OpeningBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Betting'), Width: 100, Name: "Betting", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Outstanding'), Width: 100, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('TransferAmt'), Width: 100, Name: "TransferAmt", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column',
                Formatter: function (rowIndex, rowData, cell, cellFormated) {
                    return ("<a class='num-link' href='javascript:report.statement.viewTransferDetail({0}, \"{1}\");'>" + cellFormated + "</a>").format(rowData.CustId, rowData.Username);
                }
            },
            { HeaderText: language.getString('ClosingBalance'), Width: 100, Name: "ClosingBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        gridExpandHeight: 125,
        showNumberColumn: true,
        showPager: true,
        autoResizeColumn: true
    };
    /* jshint +W101 */

    return {
        init: function () {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
            $('form').submit(function (e) {
                report.statement.submit();
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
                        var gridOptions = pageContext.IsLastLevel ? memberGridOptions : agentGridOptions;
                        gridOptions.pagerOption = {
                            itemsPerPage: core.pageSizes[2],
                            currentPage: 0,
                            itemsPerPageArray: core.pageSizes,
                            totalItem: data.List.length
                        };

                        gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
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
        viewDetail: function (agentId, userName) {
            var url = core.resolveUrl('~/Report/Statement/Detail', {
                agentId: agentId,
                fromDate: $('#FromDate').val(),
                toDate: $('#ToDate').val(),
                userName: userName,
                isMember: pageContext.IsLastLevel,
                parentUsername: $('#Username').val()
            });
            location.href = url;
        },
        viewTransferDetail: function (custId, userName) {
            var url = core.resolveUrl('~/Report/Statement/TransferDetail', {
                custId: custId,
                fromDate: $('#FromDate').val(),
                toDate: $('#ToDate').val(),
                userName: userName,
                isMember: pageContext.IsLastLevel
            });
            popup.openFrame(url, {
                width: 550,
                height: 400
            });
        }
    };
})(jQuery);