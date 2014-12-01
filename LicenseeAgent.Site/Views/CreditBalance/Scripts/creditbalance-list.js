/*global CORE, LANGUAGE, POPUP, GRID, context*/
/*exported CREDIT_BALANCE_LIST*/
'use strict';

var CREDIT_BALANCE_LIST = (function ($this, $) {
    /* jshint -W101 */
    var formatStatus = function (rowIndex, rowData, cell) {
        return cell.Value
            ? '<div class="account-status-open">' + LANGUAGE.getString('Open') + '</div>'
            : '<div class="account-status-closed">' + LANGUAGE.getString('Closed') + '</div>';
    };

    var formatCustomerName = function (rowIndex, rowData) {
        if ($.trim(rowData.CustomerName) === '') {
            rowData.CustomerName = '---';
        }
        return '{0}'.format(rowData.CustomerName);
    };

    var agentGridOptions = {
        columns: [
            { HeaderText: "#", Width: 30, Name: "RowNo", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Username'), Width: 100, Name: "UserName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 200 },
            { HeaderText: LANGUAGE.getString('FullName'), Width: 125, Name: "CustomerName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 300, Formatter: formatCustomerName },
            { HeaderText: LANGUAGE.getString('Status'), Width: 55, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false, Formatter: formatStatus },
            { HeaderText: LANGUAGE.getString('GivenCredit'), Width: 80, Name: "GivenCredit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", MergeHeaderColNumber: 2 },
            { HeaderText: "", Width: 20, Name: "TransferIcon", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData) {
                    return '<i class="icon-edit-credit-balance" onclick="CREDIT_BALANCE_LIST.editAgent({0})" title="{1}"></i>'.format(rowData.AgentId, LANGUAGE.getString('Edit'));
                }
            },
            { HeaderText: LANGUAGE.getString('UsedCredit'), Width: 80, Name: "UsedCredit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('YesterdayBalance'), Width: 80, Name: "YesterdayBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('TotalBalance'), Width: 80, Name: "TotalBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('YesterdayCash'), Width: 80, Name: "YesterdayCash", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('TotalCash'), Width: 80, Name: "TotalCash", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Outstanding'), Width: 80, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        showPager: true,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Value: LANGUAGE.getString('Total'), Name: "RowNo", CellAlign: "center", MergeColNumber: 4, NonCalculated: true },
                    { Value: "GC", Name: "GivenCredit", MergeColNumber: 2 },
                    { Value: "UC", Name: "UsedCredit" },
                    { Value: "YB", Name: "YesterdayBalance" },
                    { Value: "TB", Name: "TotalBalance" },
                    { Value: "YC", Name: "YesterdayCash" },
                    { Value: "TC", Name: "TotalCash" },
                    { Value: "O", Name: "Outstanding" }
                ]
            }
        ],
        autoResizeColumn: true
    };

    var memberGridOptions = {
        columns: [
            { HeaderText: "#", Width: 30, Name: "RowNo", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Username'), Width: 100, Name: "UserName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 200 },
            { HeaderText: LANGUAGE.getString('FullName'), Width: 125, Name: "CustomerName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 300, Formatter: formatCustomerName },
            { HeaderText: LANGUAGE.getString('Status'), Width: 55, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false, Formatter: formatStatus },
            { HeaderText: LANGUAGE.getString('GivenCredit'), Width: 80, Name: "GivenCredit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", MergeHeaderColNumber: 2 },
            { HeaderText: "", Width: 20, Name: "TransferIcon", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData) {
                    return '<i class="icon-edit-credit-balance" onclick="CREDIT_BALANCE_LIST.editMember({0})" title="{1}"></i>'.format(rowData.CustId, LANGUAGE.getString('Edit'));
                }
            },
            { HeaderText: LANGUAGE.getString('BetCredit'), Width: 80, Name: "BetCredit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('YesterdayBalance'), Width: 80, Name: "YesterdayBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('TotalBalance'), Width: 80, Name: "TotalBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
            { HeaderText: LANGUAGE.getString('Outstanding'), Width: 80, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
        ],
        showPager: true,
        showFooter: true,
        footerRows: [
            {
                Columns: [
                    { Value: "Total", Name: "RowNo", CellAlign: "center", MergeColNumber: 4, NonCalculated: true },
                    { Value: "GC", Name: "GivenCredit", MergeColNumber: 2 },
                    { Value: "BC", Name: "BetCredit" },
                    { Value: "YesterBalance", Name: "YesterdayBalance" },
                    { Value: "TB", Name: "TotalBalance" },
                    { Value: "O", Name: "Outstanding" }
                ]
            }
        ],
        autoResizeColumn: true,
        allowTruncate: true
    };
    /* jshint +W101 */

    var records;
    var gridOptions;

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

        $('form').submit(function (e) {
            submit();
            e.preventDefault();
        });

        $('form').submit();
    };

    $this.editAgent = function (agentId) {
        var url = CORE.resolveUrl('~/CreditBalance/EditAgent', {
            agentId: agentId
        });
        POPUP.openFrame(url);
        POPUP.on('edit-agent-credit', function () {
            $('form').submit();
        });
    };

    $this.editMember = function (custId) {
        var url = CORE.resolveUrl('~/CreditBalance/EditMember', {
            custId: custId
        });
        POPUP.openFrame(url);
        POPUP.on('edit-member-credit', function () {
            $('form').submit();
        });
    };

    return $this;
} (CREDIT_BALANCE_LIST || {}, jQuery));