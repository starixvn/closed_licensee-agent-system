/*global CORE, LANGUAGE, POPUP, GRID, context */
/*exported TRANSFER_LIST*/
'use strict';

var TRANSFER_LIST = (function ($this, $) {
    /* jshint -W101 */
    var formatStatus = function (rowIndex, rowData, cell) {
        return cell.Value
            ? '<div class="account-status-open">' + LANGUAGE.getString('Open') + '</div>'
            : '<div class="account-status-closed">' + LANGUAGE.getString('Closed') + '</div>';
    };

    var memberGridOptions = {
        columns: [
        /*{ HeaderText: '<i class="icon-uncheck-all" onclick="TRANSFER_LIST.toggleAll(this);"></i><i id="select-dropdown" class="icon-arrow"></i>',
        Width: 30, Name: "Select", CellAlign: "center",
        Formatter: function () {
        return '<span class="cell-select icon icon-uncheck" onclick="TRANSFER_LIST.toggleCheck(this);"></span>';
        }
        },*/
            {HeaderText: "#", Width: 30, Name: "AgentId", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Username'), Width: 100, Name: "UserName", HeaderAlign: "center", AllowTruncate: true, CanResize: false },
            { HeaderText: LANGUAGE.getString('FullName'), Width: 125, Name: "CustomerName", HeaderAlign: "center", AllowTruncate: true, CanResize: false },
            { HeaderText: LANGUAGE.getString('Status'), Width: 55, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false, Formatter: formatStatus },
            { HeaderText: LANGUAGE.getString('YesterdayBalance'), Width: 80, Name: "YesterdayBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', MergeHeaderColNumber: 2 },
            { HeaderText: "", Width: 20, Name: "TransferIcon", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData) {
                    return '<i class="icon-transfer" onclick="TRANSFER_LIST.transferMember({0})" title="{1}"></i>'.format(rowData.CustId, LANGUAGE.getString('Transfer'));
                }
            },
            { HeaderText: LANGUAGE.getString('TotalBalance'), Width: 80, Name: "TotalBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Outstanding'), Width: 80, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('GivenCredit'), Width: 80, Name: "GivenCredit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }
        ],
        showPager: true,
        showNumberColumn: true,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Value: LANGUAGE.getString("Total"), Name: "AgentId", MergeColNumber: 4, NonCalculated: true },
                    { Value: LANGUAGE.getString("YesterdayBalance"), Name: "YesterdayBalance", MergeColNumber: 2 },
                    { Value: LANGUAGE.getString("TotalBalance"), Name: "TotalBalance" },
                    { Value: "", Name: "Outstanding", MergeColNumber: 2, NonCalculated: true }
                ]
            }
        ],
        autoResizeColumn: true
    };

    var agentGridOptions = {
        columns: [
        /*{ HeaderText: '<i class="icon-uncheck-all" onclick="TRANSFER_LIST.toggleAll(this);"></i><i id="select-dropdown" class="icon-arrow"></i>',
        Width: 30, Name: "Select", CellAlign: "center",
        Formatter: function () {
        return '<span class="cell-select icon icon-uncheck" onclick="TRANSFER_LIST.toggleCheck(this);"></span>';
        }
        },*/
            {HeaderText: "#", Width: 30, Name: "AgentId", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Username'), Width: 100, Name: "UserName", HeaderAlign: "center", AllowTruncate: true },
            { HeaderText: LANGUAGE.getString('FullName'), Width: 125, Name: "CustomerName", HeaderAlign: "center", AllowTruncate: true },
            { HeaderText: LANGUAGE.getString('Status'), Width: 55, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false, Formatter: formatStatus },
            { HeaderText: LANGUAGE.getString('YesterdayBalance'), Width: 80, Name: "YesterdayBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$', MergeHeaderColNumber: 2 },
            { HeaderText: "", Width: 20, Name: "TransferIcon", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData) {
                    return '<i class="icon-transfer" onclick="TRANSFER_LIST.transferAgent({0})" title="{1}"></i>'.format(rowData.AgentId, LANGUAGE.getString('Transfer'));
                }
            },
            { HeaderText: LANGUAGE.getString('TotalBalance'), Width: 80, Name: "TotalBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('YesterdayCash'), Width: 80, Name: "YesterdayCash", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('TotalCash'), Width: 80, Name: "TotalCash", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Outstanding'), Width: 80, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('GivenCredit'), Width: 80, Name: "GivenCredit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('UsedCredit'), Width: 80, Name: "UsedCredit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }
        ],
        showPager: true,
        showNumberColumn: true,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Value: LANGUAGE.getString("Total"), Name: "AgentId", MergeColNumber: 4, NonCalculated: true },
                    { Value: "YB", Name: "YesterdayBalance", MergeColNumber: 2 },
                    { Value: "TB", Name: "TotalBalance" },
                    { Value: "YC", Name: "YesterdayCash" },
                    { Value: "TC", Name: "TotalCash" },
                    { Value: "", Name: "Outstanding", MergeColNumber: 3, NonCalculated: true }
                ]
            }
        ],
        autoResizeColumn: true
    };
    /* jshint +W101 */

    var gridOptions;
    var records;
    var submit = function () {
        CORE.ajaxJSON({
            url: $('form').attr('action'),
            data: $('form').serialize()
        }, function (data) {
            records = data.List;
            pagerSelectCallBack(0, GRID.PageSizes[2]);
        });
    };

    var pagerSelectCallBack = function (pageIndex, itemsPerPage) {
        gridOptions.pagerOption = {
            itemsPerPage: itemsPerPage,
            currentPage: pageIndex,
            totalItem: records.length,
            itemsPerPageArray: GRID.PageSizes,
            showDetail: true,
            customRenderFooter: function (bodyRows) {
                return GRID.generateTotalRows(bodyRows, gridOptions);
            }
        };
        gridOptions.tdCssClass = function (rowIndex, colName, rowData) {
            if (colName === 'YesterdayBalance' || colName === 'TransferIcon') {
                return rowData.IsHighlighted ? 'text-highlighted' : '';
            }
        };
        gridOptions.bodyRows = GRID.generateBodyRows(records, gridOptions);
        $('#grid').show().fanexGrid(gridOptions);
    };

    $this.init = function (isShowMember) {
        gridOptions = isShowMember ? memberGridOptions : agentGridOptions;
        $('.selectpicker').selectpicker();

        context.init({
            preventDoubleContext: false
        });
        context.attach('#select-dropdown', [
            { text: LANGUAGE.getString('All'), href: '#', action: $this.checkAll },
            { text: LANGUAGE.getString('None'), href: '#', action: $this.uncheckAll }
        ]);

        $('form').submit(function (e) {
            submit();
            e.preventDefault();
        });

        $('form').submit();
    };

    $this.toggleCheck = function (target) {
        if ($(target).hasClass("icon-uncheck")) {
            $(target).attr("class", "icon-checked");
        }
        else {
            $(target).attr("class", "icon-uncheck");
        }

        if ($('.icon-uncheck').length === 0) {
            $(".icon-uncheck-all").attr("class", "icon-checked-all");
        }
        else {
            $(".icon-checked-all").attr("class", "icon-uncheck-all");
        }
    };

    $this.toggleAll = function (target) {
        if ($(target).hasClass("icon-uncheck-all")) {
            $this.checkAll();
        }
        else {
            $this.uncheckAll();
        }
    };

    $this.checkAll = function () {
        $(".icon-uncheck-all").attr("class", "icon-checked-all");
        $(".icon-uncheck").attr("class", "icon-checked");
    };

    $this.uncheckAll = function () {
        $(".icon-checked-all").attr("class", "icon-uncheck-all");
        $(".icon-checked").attr("class", "icon-uncheck");
    };

    $this.transferAgent = function (agentId) {
        var url = CORE.resolveUrl('~/Transfer/TransferAgent', {
            agentId: agentId
        });
        POPUP.openFrame(url);
        POPUP.on('transfer-agent', function () {
            $('form').submit();
        });
    };

    $this.transferMember = function (memberId) {
        var url = CORE.resolveUrl('~/Transfer/TransferMember', {
            memberId: memberId
        });
        POPUP.openFrame(url);
        POPUP.on('transfer-member', function () {
            $('form').submit();
        });
    };

    return $this;
} (TRANSFER_LIST || {}, jQuery));