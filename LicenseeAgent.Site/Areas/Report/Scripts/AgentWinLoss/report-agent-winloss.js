/*global CORE, LANGUAGE, ENUMS, GRID, BREADCRUMB, BET_LIST, COMPONENT*/

var REPORT = REPORT || {};
REPORT.AGENT_WINLOSS = (function ($this, $) {
    'use strict';

    var renderUsername = function (rowIndex, rowData, cell) {
        if (rowData.CustId !== 0) {
            return '<a href="#" onclick="REPORT.AGENT_WINLOSS.showBetList({0}, \'{1}\');">{1}</a>'.format(rowData.CustId, cell.Value);
        } else if (rowData.AgentId === 0) {
            return '<a href="#" class="text-direct-member" onclick="REPORT.AGENT_WINLOSS.showMember();">{0}</a>'.format(LANGUAGE.getString('DirectMembers'));
        } else {
            return '<a href="#" onclick="REPORT.AGENT_WINLOSS.showAgent({0}, \'{1}\');">{1}</a>'.format(rowData.AgentId, cell.Value);
        }
    };

    /* jshint -W101 */
    var gridOptionsForAgentCash = {
        customizeHeader: true,
        groupHeaderRows: [
            {
                Columns: [
                    { HeaderText: LANGUAGE.getString('Agent'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: LANGUAGE.getString('Member'), Name: "BetCount", MergeColNumber: 5 },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 1 }
                ]
            }
        ],
        columns: [
            { HeaderText: LANGUAGE.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
            { HeaderText: LANGUAGE.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true },
            { HeaderText: LANGUAGE.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "Winlost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "PlayerComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: LANGUAGE.getString('Total'), CellAlign: "center", NonCalculated: true },
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
                    { HeaderText: LANGUAGE.getString('Agent'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: LANGUAGE.getString('Member'), Name: "BetCount", MergeColNumber: 3 },
                    { HeaderText: LANGUAGE.getString('Downline'), Name: "DownlineStake", MergeColNumber: 2, ThClass: 'alter-column' },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 3 },
                    { HeaderText: LANGUAGE.getString('Upline'), Name: "UplineStake", MergeColNumber: 2, ThClass: 'alter-column' }
                ]
            }
        ],
        columns: [
            { HeaderText: LANGUAGE.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
            { HeaderText: LANGUAGE.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('EffectiveTurnover'), Width: 100, Name: "DownlineStake", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "DownlineTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "AComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "ATotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('EffectiveTurnover'), Width: 100, Name: "UplineStake", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "UplineTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' }
        ],
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: LANGUAGE.getString('Total'), CellAlign: "center", NonCalculated: true },
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
                    { HeaderText: LANGUAGE.getString('Agent'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: LANGUAGE.getString('Member'), Name: "BetCount", MergeColNumber: 5 },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 3 }
                ]
            }
        ],
        columns: [
            { HeaderText: LANGUAGE.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
            { HeaderText: LANGUAGE.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true },
            { HeaderText: LANGUAGE.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "Winlost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "PlayerComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "AComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "ATotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }

        ],
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: LANGUAGE.getString('Total'), CellAlign: "center", NonCalculated: true },
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
                    { HeaderText: LANGUAGE.getString('Agent'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: LANGUAGE.getString('Member'), Name: "BetCount", MergeColNumber: 3 },
                    { HeaderText: LANGUAGE.getString('Downline'), Name: "DownlineStake", MergeColNumber: 2, ThClass: 'alter-column' },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 3 }
                ]
            }
        ],
        columns: [
            { HeaderText: LANGUAGE.getString('Agent'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
            { HeaderText: LANGUAGE.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true },
            { HeaderText: LANGUAGE.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('EffectiveTurnover'), Width: 100, Name: "DownlineStake", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "DownlineTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "AComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "ATotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: LANGUAGE.getString('Total'), CellAlign: "center", NonCalculated: true },
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
                    { HeaderText: LANGUAGE.getString('Member'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: LANGUAGE.getString('Member'), Name: "BetCount", MergeColNumber: 5 },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 1 }
                ]
            }
        ],
        columns: [
            { HeaderText: LANGUAGE.getString('Member'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
            { HeaderText: LANGUAGE.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true },
            { HeaderText: LANGUAGE.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "Winlost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "PlayerComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: LANGUAGE.getString('Total'), CellAlign: "center", NonCalculated: true },
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
                    { HeaderText: LANGUAGE.getString('Member'), Name: "Username", MergeRowNumber: 2 },
                    { HeaderText: LANGUAGE.getString('Member'), Name: "BetCount", MergeColNumber: 5 },
                    { HeaderText: 'Model.UserName', Name: "AWinLost", MergeColNumber: 3, ThClass: 'alter-column' },
                    { HeaderText: LANGUAGE.getString('Upline'), Name: "UplineStake", MergeColNumber: 2 }
                ]
            }
        ],
        columns: [
            { HeaderText: LANGUAGE.getString('Member'), Width: 100, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter: renderUsername },
            { HeaderText: LANGUAGE.getString('BetCount'), Width: 70, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true },
            { HeaderText: LANGUAGE.getString('Turnover'), Width: 100, Name: "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "Winlost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "PlayerComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "MemberTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('WinLoss'), Width: 100, Name: "AWinLost", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('Commission'), Width: 100, Name: "AComm", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "ATotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', ThClass: 'alter-column' },
            { HeaderText: LANGUAGE.getString('EffectiveTurnover'), Width: 100, Name: "UplineStake", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('Total'), Width: 100, Name: "UplineTotal", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }
        ],
        showNumberColumn: false,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Name: "Username", Value: LANGUAGE.getString('Total'), CellAlign: "center", NonCalculated: true },
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

    $this.init = function () {
        $("#tabs-area a:first").tab('show');
        $('.datepicker').datepicker();

        var breadcrumb = BREADCRUMB.init();
        breadcrumb.on('itemClicked', function (item, data) {
            if (data.AgentId > 0) {
                showData(data.AgentId, false);
                var lastItem = BREADCRUMB.getLastItem();
                lastItem.IsLink = false;
                BREADCRUMB.render();
            }
        });

        registerButtonClickEvent();
        COMPONENT.registerCheckboxProductEvent();
        $('form').submit();
    };

    var registerButtonClickEvent = function () {
        $('form').submit(function (e) {
            var agentId = $('#AgentId').val();
            var showMembers = $('#ShowMembers').val();
            showData(agentId, showMembers);
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
    };

    $this.showMember = function () {
        var agentId = $('#AgentId').val();
        var item = BREADCRUMB.getLastItem();
        item.IsLink = true;
        BREADCRUMB.render();
        showData(agentId, true);
    };

    $this.showAgent = function (agentId, userName) {
        BREADCRUMB.addItem({
            Text: userName,
            Data: {
                AgentId: agentId
            }
        });
        showData(agentId, false);
    };

    var showData = function (agentId, showMembers) {
        // update form input
        $('#AgentId').val(agentId);
        $('#ShowMembers').val(showMembers);
        COMPONENT.setProductValue();
        CORE.ajaxJSON({
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
            gridOptions.bodyRows = GRID.generateBodyRows(data.List, gridOptions);
            GRID.generateTotalRows(gridOptions.bodyRows, gridOptions);
            $('#grid').show().fanexGrid(gridOptions);
        });
    };

    $this.showBetList = function (custId, userName) {
        BREADCRUMB.addItem({
            Text: userName,
            Data: {
                CustId: custId
            }
        });

        COMPONENT.setProductValue();
        var input = $('form').serializeObject();
        input.CustId = custId;
        var settledNoDraw = [ENUMS.TicketStatus.Won, ENUMS.TicketStatus.Lose].join(',');
        var settledHasDraw = [ENUMS.TicketStatus.Draw, ENUMS.TicketStatus.Won, ENUMS.TicketStatus.Lose].join(',');
        var drawFilter = $('input[name=DrawFilter]:checked', '#search-form').val();
        input.Status = drawFilter === "1" ? settledNoDraw : settledHasDraw;
        BET_LIST.showByMember({
            Input: input,
            ShowTotalRow: true
        });
    };

    return $this;
} (REPORT.AGENT_WINLOSS || {}, jQuery));