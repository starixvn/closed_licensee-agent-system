/*global language, core, pageContext*/

var report = report || {};
report.agentWinLoss = (function ($) {
    'use strict';

    /* jshint -W101 */
    var gridOptionsForAgentCash = {
        customizeHeader: true,
        groupHeaderRows: [
            {
                Columns: [
                    { HeaderText: language.getString('Agent'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: language.getString('Member'), Name: "BetCount", MergeColNumber: 5 },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 1 }
                ]
            }
        ],
        columns: [
            { HeaderText: language.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell) {
                    return ("<a href='javascript:report.agentWinLoss.viewDownline({0}, {1}, \"{2}\");'>" + cell.Value + "</a>").format(rowData.AgentId, rowData.AgentLevel, cell.Value);
                }
            },
            { HeaderText: language.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true },
            { HeaderText: language.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "Winlost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "PlayerComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        gridExpandHeight: 225,
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: language.getString('Total'), CellAlign: "center", NonCalculated: true },
                    { Name: "BetCount" },
                    { Name: "Turnover" },
                    { Name: "Winlost" },
                    { Name: "PlayerComm" },
                    { Name: "MemberTotal" },
                    { Name: "AWinLost" }
                ]
            }
        ],
        autoResizeColumn: true
    };

    var gridOptionsForAgentCredit = {
        customizeHeader: true,
        groupHeaderRows: [
            {
                Columns: [
                        { HeaderText: language.getString('Agent'), Name: "Username", MergeRowNumber: 2 },
                        { HeaderText: language.getString('Member'), Name: "BetCount", MergeColNumber: 3 },
                        { HeaderText: language.getString('Downline'), Name: "DownlineStake", MergeColNumber: 2, ThClass: 'alter-column' },
                        { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 3 },
                        { HeaderText: language.getString('Upline'), Name: "UplineStake", MergeColNumber: 2, ThClass: 'alter-column' }
                    ]
            }
        ],
        columns: [
            { HeaderText: language.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell) {
                    return ("<a href='javascript:report.agentWinLoss.viewDownline({0}, {1}, \"{2}\");'>" + cell.Value + "</a>").format(rowData.AgentId, rowData.AgentLevel, cell.Value);
                }
            },
            { HeaderText: language.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: language.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('EffectiveTurnover'), Width: 100, Name: "DownlineStake", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' },
            { HeaderText: language.getString('Total'), Width: 100, Name: "DownlineTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "AComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Total'), Width: 100, Name: "ATotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('EffectiveTurnover'), Width: 100, Name: "UplineStake", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' },
            { HeaderText: language.getString('Total'), Width: 100, Name: "UplineTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' }
        ],
        gridExpandHeight: 225,
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: language.getString('Total'), CellAlign: "center", NonCalculated: true },
                    { Name: "BetCount" },
                    { Name: "Turnover" },
                    { Name: "MemberTotal" },
                    { Name: "DownlineStake" },
                    { Name: "DownlineTotal" },
                    { Name: "AWinLost" },
                    { Name: "AComm" },
                    { Name: "ATotal" },
                    { Name: "UplineStake" },
                    { Name: "UplineTotal" }
                ]
            }
        ],
        autoResizeColumn: true
    };

    var gridOptionsForLicenseeCash = {
        customizeHeader: true,
        groupHeaderRows: [
            {
                Columns: [
                    { HeaderText: language.getString('Agent'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: language.getString('Member'), Name: "BetCount", MergeColNumber: 5 },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 3 }
                ]
            }
        ],
        columns: [
            { HeaderText: language.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell) {
                    return ("<a href='javascript:report.agentWinLoss.viewDownline({0}, {1}, \"{2}\");'>" + cell.Value + "</a>").format(rowData.AgentId, rowData.AgentLevel, cell.Value);
                }
            },
            { HeaderText: language.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true },
            { HeaderText: language.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "Winlost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "PlayerComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "AComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Total'), Width: 100, Name: "ATotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }

        ],
        gridExpandHeight: 225,
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: language.getString('Total'), CellAlign: "center", NonCalculated: true },
                    { Name: "BetCount" },
                    { Name: "Turnover" },
                    { Name: "Winlost" },
                    { Name: "PlayerComm" },
                    { Name: "MemberTotal" },
                    { Name: "AWinLost" },
                    { Name: "AComm" },
                    { Name: "ATotal" }
                ]
            }
        ],
        autoResizeColumn: true
    };

    var gridOptionsForLicenseeCredit = {
        customizeHeader: true,
        groupHeaderRows: [
            {
                Columns: [
                    { HeaderText: language.getString('Agent'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: language.getString('Member'), Name: "BetCount", MergeColNumber: 3 },
                    { HeaderText: language.getString('Downline'), Name: "DownlineStake", MergeColNumber: 2, ThClass: 'alter-column' },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 3 }
                ]
            }
        ],
        columns: [
            { HeaderText: language.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell) {
                    return ("<a href='javascript:report.agentWinLoss.viewDownline({0}, {1}, \"{2}\");'>" + cell.Value + "</a>").format(rowData.AgentId, rowData.AgentLevel, cell.Value);
                }
            },
            { HeaderText: language.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true },
            { HeaderText: language.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('EffectiveTurnover'), Width: 100, Name: "DownlineStake", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' },
            { HeaderText: language.getString('Total'), Width: 100, Name: "DownlineTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "AComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: language.getString('Total'), Width: 100, Name: "ATotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        gridExpandHeight: 225,
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: language.getString('Total'), CellAlign: "center", NonCalculated: true },
                    { Name: "BetCount" },
                    { Name: "Turnover" },
                    { Name: "MemberTotal" },
                    { Name: "DownlineStake" },
                    { Name: "DownlineTotal" },
                    { Name: "AWinLost" },
                    { Name: "AComm" },
                    { Name: "ATotal" }
                ]
            }
        ],
        autoResizeColumn: true
    };

    var gridOptionsForMemberCash = {
        customizeHeader: true,
        groupHeaderRows: [
            {
                Columns: [
                    { HeaderText: language.getString('Member'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: language.getString('Member'), Name: "BetCount", MergeColNumber: 5 },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 1 }
                ]
            }
        ],
        columns: [
            { HeaderText: language.getString('Member'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell) {
                    return ("<a href='javascript:report.agentWinLoss.viewBetList({0}, {1}, \"{2}\");'>" + cell.Value + "</a>").format(rowData.CustId, rowData.AgentLevel, cell.Value);
                }
            },
            { HeaderText: language.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true },
            { HeaderText: language.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "Winlost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "PlayerComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        gridExpandHeight: 225,
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: language.getString('Total'), CellAlign: "center", NonCalculated: true },
                    { Name: "BetCount" },
                    { Name: "Turnover" },
                    { Name: "Winlost" },
                    { Name: "PlayerComm" },
                    { Name: "MemberTotal" },
                    { Name: "AWinLost" }
                ]
            }
        ],
        autoResizeColumn: true
    };

    var gridOptionsForMemberCredit = {
        customizeHeader: true,
        groupHeaderRows: [
            {
                Columns: [
                    { HeaderText: language.getString('Member'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: language.getString('Member'), Name: "BetCount", MergeColNumber: 5 },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 3, ThClass: 'alter-column' },
                    { HeaderText: language.getString('Upline'), Name: "UplineStake", MergeColNumber: 2 }
                ]
            }
        ],
        columns: [
            { HeaderText: language.getString('Member'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left",
                Formatter: function (rowIndex, rowData, cell) {
                    return ("<a href='javascript:report.agentWinLoss.viewBetList({0}, {1}, \"{2}\");'>" + cell.Value + "</a>").format(rowData.CustId, rowData.AgentLevel, cell.Value);
                }
            },
            { HeaderText: language.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true },
            { HeaderText: language.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "Winlost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "PlayerComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('Commission'), Width: 100, Name: "AComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('Total'), Width: 100, Name: "ATotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: language.getString('EffectiveTurnover'), Width: 100, Name: "UplineStake", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString('Total'), Width: 100, Name: "UplineTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }
        ],
        gridExpandHeight: 225,
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: language.getString('Total'), CellAlign: "center", NonCalculated: true },
                    { Name: "BetCount" },
                    { Name: "Turnover" },
                    { Name: "Winlost" },
                    { Name: "PlayerComm" },
                    { Name: "MemberTotal" },
                    { Name: "AWinLost" },
                    { Name: "AComm" },
                    { Name: "ATotal" },
                    { Name: "UplineStake" },
                    { Name: "UplineTotal" }
                ]
            }
        ],
        autoResizeColumn: true
    };
    /* jshint +W101 */

    var betListUrl = '~/BetList/BetListWinlossReport';
    var gridOptions;
    return {
        init: function () {
            report.agentWinLoss.initGridOptions();
            $("#tabs-area a:first").tab('show');
            report.agentWinLoss.initDatePicker();
            report.agentWinLoss.registerButtonClickEvent();
            report.registerCheckboxProductEvent();
            report.agentWinLoss.dataGridBinding();
        },
        initGridOptions: function () {
            if (pageContext.IsLastLevel) {
                if (pageContext.IsDeposit) {
                    gridOptionsForMemberCash.groupHeaderRows[0].Columns[2].HeaderText = pageContext.AgentName;
                    gridOptions = gridOptionsForMemberCash;
                } else {
                    gridOptionsForMemberCredit.groupHeaderRows[0].Columns[2].HeaderText = pageContext.AgentName;
                    gridOptions = gridOptionsForMemberCredit;
                }
            } else {
                if (pageContext.AgentLevel === 1) {
                    if (pageContext.IsDeposit) {
                        gridOptionsForLicenseeCash.groupHeaderRows[0].Columns[2].HeaderText = pageContext.AgentName;
                        gridOptions = gridOptionsForLicenseeCash;
                    } else {
                        gridOptionsForLicenseeCredit.groupHeaderRows[0].Columns[3].HeaderText = pageContext.AgentName;
                        gridOptions = gridOptionsForLicenseeCredit;
                    }
                } else {
                    if (pageContext.IsDeposit) {
                        gridOptionsForAgentCash.groupHeaderRows[0].Columns[2].HeaderText = pageContext.AgentName;
                        gridOptions = gridOptionsForAgentCash;
                    } else {
                        gridOptionsForAgentCredit.groupHeaderRows[0].Columns[3].HeaderText = pageContext.AgentName;
                        gridOptions = gridOptionsForAgentCredit;
                    }
                }
            }
        },
        initDatePicker: function () {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
        },
        registerButtonClickEvent: function () {
            $('form').submit(function (e) {
                report.agentWinLoss.dataGridBinding();
                e.preventDefault();
            });

            $('#today').click(function () {
                var today = $(this).data('date');
                $('#fromDatePicker').datepicker('update', today);
                $('#toDatePicker').datepicker('update', today);
                $('form').submit();
            });
            $('#yesterday').click(function () {
                var yesterday = $(this).data('date');
                $('#fromDatePicker').datepicker('update', yesterday);
                $('#toDatePicker').datepicker('update', yesterday);
                $('form').submit();
            });

            // breadcrum
            $('.breadcrumb a').click(function () {
                var agentId = $(this).data('agent-id');
                var agentLevel = $(this).data('agent-level');
                var agentName = $(this).text();
                report.agentWinLoss.viewDownline(agentId, agentLevel, agentName);
            });
        },
        dataGridBinding: function () {
            report.setProductValue();

            var submitOptions = $('form').serializeObject();
            core.ajaxJSON({
                url: $('form').attr('action'),
                data: jQuery.param(submitOptions)
            }, function (data) {
                gridOptions.bodyRows = core.genGridRows(data, gridOptions);
                core.genGridTotalRows(gridOptions.bodyRows, gridOptions);
                $('#grid').show().fanexGrid(gridOptions);
            });
        },
        viewDownline: function (agentId, agentLevel, username) {
            report.setProductValue();
            $("#BreadCrumb").setBreadCrumb(agentId, agentLevel, username);
            location.href = $('form').attr('action') + '?' + $.param({
                agentId: agentId,
                agentLevel: agentLevel,
                username: username,
                drawFilter: $('input[name=DrawFilter]:checked', '#search-form').val(),
                breadcrumb: $("#BreadCrumb").val(),
                FromDate: $("#FromDate").val(),
                ToDate: $("#ToDate").val(),
                Product: $("#Product").val()
            });
        },
        viewBetList: function (custId, agentLevel, username) {
            report.setProductValue(); // Set Product selected values.
            $("#BreadCrumb").setBreadCrumb(custId, agentLevel, username);
            var drawFilter = $('input[name=DrawFilter]:checked', '#search-form').val();
            location.href = core.resolveUrl(betListUrl, {
                custId: custId,
                breadcrumb: $("#BreadCrumb").val(),
                FromDate: $("#FromDate").val(),
                ToDate: $("#ToDate").val(),
                drawFilter: drawFilter,
                statusIDs: (drawFilter === "1" ? report.SettledNoDraw : report.SettledHasDraw),
                ProductIds: $("#Product").val(),
                title: document.title
            });
        }
    };
})(jQuery);