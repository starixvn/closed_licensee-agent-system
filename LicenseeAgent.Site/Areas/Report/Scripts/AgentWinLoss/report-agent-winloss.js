/*global language, core*/

var report = report || {};
report.agentWinLoss = (function ($) {
    'use strict';

    var renderUsername = function (rowIndex, rowData, cell) {
        if (rowData.CustId !== 0) {
            return '<a href="#" onclick="report.agentWinLoss.showBetList({0}, \'{1}\');">{1}</a>'.format(rowData.CustId, cell.Value);
        } else if (rowData.AgentId === 0) {
            return '<a href="#" class="text-direct-member" onclick="report.agentWinLoss.showMember();">{0}</a>'.format(language.getString('DirectMembers'));
        } else {
            return '<a href="#" onclick="report.agentWinLoss.showAgent({0}, \'{1}\');">{1}</a>'.format(rowData.AgentId, cell.Value);
        }
    };

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
            { HeaderText: language.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
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
            { HeaderText: language.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
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
            { HeaderText: language.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
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
            { HeaderText: language.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
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
            { HeaderText: language.getString('Member'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
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
            { HeaderText: language.getString('Member'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
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

    return {
        init: function () {
            $("#tabs-area a:first").tab('show');
            $('.datepicker').datepicker();

            var breadcrumb = core.breadcrumb.init();
            breadcrumb.on('itemClicked', function (item, data) {
                if (data.AgentId > 0) {
                    report.agentWinLoss.showData(data.AgentId, false);
                    var lastItem = core.breadcrumb.getLastItem();
                    lastItem.IsLink = false;
                    core.breadcrumb.render();
                }
            });

            report.agentWinLoss.registerButtonClickEvent();
            report.registerCheckboxProductEvent();
            $('form').submit();
        },
        registerButtonClickEvent: function () {
            $('form').submit(function (e) {
                var agentId = $('#AgentId').val();
                var showMembers = $('#ShowMembers').val();
                report.agentWinLoss.showData(agentId, showMembers);
                e.preventDefault();
            });

            $('#btnToday').click(function () {
                var today = $(this).data('date');
                $('#dpFromDate').datepicker('update', today);
                $('#dpToDate').datepicker('update', today);
                $('form').submit();
            });
            $('#btnYesterday').click(function () {
                var yesterday = $(this).data('date');
                $('#dpFromDate').datepicker('setDates', yesterday);
                $('#dpToDate').datepicker('setDates', yesterday);
                $('form').submit();
            });
        },
        showMember: function () {
            var agentId = $('#AgentId').val();
            var item = core.breadcrumb.getLastItem();
            item.IsLink = true;
            core.breadcrumb.render();
            report.agentWinLoss.showData(agentId, true);
        },
        showAgent: function (agentId, userName) {
            core.breadcrumb.addItem({
                Text: userName,
                Data: {
                    AgentId: agentId
                }
            });
            report.agentWinLoss.showData(agentId, false);
        },
        showData: function (agentId, showMembers) {
            // update form input
            $('#AgentId').val(agentId);
            $('#ShowMembers').val(showMembers);
            report.setProductValue();
            core.ajaxJSON({
                url: $('form').attr('action'),
                data: $('form').serializeObject()
            }, function (data) {
                var gridOptions;
                if (data.NumberOfLevel === data.AgentLevel - 1) {
                    if (data.IsDeposit) {
                        gridOptionsForMemberCash.groupHeaderRows[0].Columns[2].HeaderText = data.AgentName;
                        gridOptions = gridOptionsForMemberCash;
                    } else {
                        gridOptionsForMemberCredit.groupHeaderRows[0].Columns[2].HeaderText = data.AgentName;
                        gridOptions = gridOptionsForMemberCredit;
                    }
                } else {
                    if (data.AgentLevel === 1) {
                        if (data.IsDeposit) {
                            gridOptionsForLicenseeCash.groupHeaderRows[0].Columns[2].HeaderText = data.AgentName;
                            gridOptions = gridOptionsForLicenseeCash;
                        } else {
                            gridOptionsForLicenseeCredit.groupHeaderRows[0].Columns[3].HeaderText = data.AgentName;
                            gridOptions = gridOptionsForLicenseeCredit;
                        }
                    } else {
                        if (data.IsDeposit) {
                            gridOptionsForAgentCash.groupHeaderRows[0].Columns[2].HeaderText = data.AgentName;
                            gridOptions = gridOptionsForAgentCash;
                        } else {
                            gridOptionsForAgentCredit.groupHeaderRows[0].Columns[3].HeaderText = data.AgentName;
                            gridOptions = gridOptionsForAgentCredit;
                        }
                    }
                }
                gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                core.genGridTotalRows(gridOptions.bodyRows, gridOptions);
                $('#grid').show().fanexGrid(gridOptions);
            });
        },
        showBetList: function (custId, userName) {
            core.breadcrumb.addItem({
                Text: userName,
                Data: {
                    CustId: custId
                }
            });

            report.setProductValue();
            var input = $('form').serializeObject();
            input.CustId = custId;
            var settledNoDraw = [core.enums.TicketStatus.Won, core.enums.TicketStatus.Lose].join(',');
            var settledHasDraw = [core.enums.TicketStatus.Draw, core.enums.TicketStatus.Won, core.enums.TicketStatus.Lose].join(',');
            var drawFilter = $('input[name=DrawFilter]:checked', '#search-form').val();
            input.Status = drawFilter === "1" ? settledNoDraw : settledHasDraw;
            report.betlist.showByMember({
                Input: input,
                ShowTotalRow: true
            });
        }
    };
})(jQuery);