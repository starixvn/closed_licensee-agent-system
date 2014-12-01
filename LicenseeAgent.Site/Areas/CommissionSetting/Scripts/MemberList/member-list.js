/*global CORE, LANGUAGE, POPUP, GRID, numeral */
'use strict';

var COMMISSION_SETTING = COMMISSION_SETTING || {};
COMMISSION_SETTING.MEMBER_LIST = (function ($this, $) {
    /* jshint -W101 */
    var baseGridOptions = {
        customizeHeader: true,
        groupHeaderRows: [
            {
                Columns: [
                    { HeaderText: '<i class="icon-uncheck-all" onclick="COMMISSION_SETTING.MEMBER_LIST.checkAllItem(this);"></i>', Name: "CustId", MergeRowNumber: 2 },
                    { HeaderText: "#", Name: "No", MergeRowNumber: 2 },
                    { HeaderText: LANGUAGE.getString('SystemId'), Name: "SystemId", MergeRowNumber: 2 },
                    { HeaderText: LANGUAGE.getString('Username'), Name: "UserName", MergeRowNumber: 2 },
                    { HeaderText: LANGUAGE.getString('Group'), Name: "GroupName", MergeRowNumber: 2 }
                ]
            }
        ],
        columns: [
            {
                Width: 30, Name: "CustId", CellAlign: "center", MergeColNumber: 2, CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    return '<span class="cell-select icon icon-uncheck" onclick="COMMISSION_SETTING.MEMBER_LIST.selectClick(this);" data-cust-id="' + cell.Value + '"></span>';
                }
            },
            { HeaderText: "#", Name: "No", Width: 60, HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('SystemId'), Width: 150, Name: "SystemId", HeaderAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Username'), Width: 150, Name: "UserName", HeaderAlign: "center" },
            { HeaderText: LANGUAGE.getString('Group'), Width: 100, Name: "GroupName", HeaderAlign: "center",
                Formatter: function (rowIndex, rowData, cell) {
                    return '<span class="group-comm">' + cell.Value + '</span>';
                }
            }
        ],
        pagerOption: {
            itemsPerPage: GRID.PageSizes[2],
            currentPage: 0,
            itemsPerPageArray: GRID.PageSizes
        },
        showPager: true,
        indexNumberColumn: 1,
        autoResizeColumn: true
    };

    var formatNumber = function (value) {
        return numeral(value * 100).format('0.00');
    };

    var generateGridColumn = function (productColumn) {
        return { HeaderText: LANGUAGE.getString(productColumn.HeaderResource), Width: 50, Name: productColumn.ColumnName, FormatNumber: "0,0.00", HeaderAlign: "center", CellAlign: "center",
            Formatter: function (rowIndex, rowData, cell) {
                return '<span data-column-name="{0}" class="commission-cell">{1}</span>'.format(productColumn.ColumnName, formatNumber(cell.Value));
            }
        };
    };

    var generateGridOptions = function (productColumns) {
        var gridOptions = $.extend(true, {}, baseGridOptions);
        gridOptions.groupHeaderRows[0].Columns.push({
            HeaderText: LANGUAGE.getString('CommissionHeader'),
            Name: productColumns[0].ColumnName,
            MergeColNumber: productColumns.length
        });
        $.each(productColumns, function (index) {
            var productColumn = this;
            var gridColumn = generateGridColumn(productColumn);
            gridOptions.columns.splice(5 + index, 0, gridColumn);
        });
        return gridOptions;
    };
    /* jshint +W101 */

    var changeButtonState = function () {
        if ($(".cell-select").hasClass("icon-checked")) {
            $("#moveToGroup").removeClass("disabled");
            $("#updateCommission").removeClass("disabled");
            $("#removeFromGroup").removeClass("disabled");
        }
        else {
            $("#moveToGroup").addClass("disabled");
            $("#updateCommission").addClass("disabled");
            $("#removeFromGroup").addClass("disabled");
        }
    };

    var getSelectedMembers = function () {
        var custIds = [];
        $(".icon-checked").each(function () {
            custIds.push($(this).data("cust-id"));
        });
        return custIds.join(',');
    };

    var updateMemberCommissions = function (memberCommissions) {
        $('.icon-checked').each(function () {
            var custId = $(this).data('cust-id');
            var filter = $.grep(memberCommissions, function (item) {
                return item.CustId === custId;
            });
            if (filter.length === 1) {
                var memberCommission = filter[0];
                var tr = $(this).parents('tr');
                tr.find('span.group-comm').text(memberCommission.GroupName);
                tr.find('span.commission-cell').each(function () {
                    var columnName = $(this).data('column-name');
                    $(this).text(formatNumber(memberCommission[columnName]));
                });
            }
        });
    };

    $this.init = function () {
        $('.selectpicker').selectpicker();

        // search
        $('#search-form').submit(function (e) {
            $this.reRenderAfterPaged(0, GRID.PageSizes[2]);
            e.preventDefault();
        });

        // move group
        $('#moveToGroup').click(function () {
            var custIds = getSelectedMembers();
            if (custIds !== '') {
                var url = CORE.resolveUrl('~/CommissionSetting/MemberList/MoveToGroup', {
                    custIds: custIds
                });
                POPUP.openFrame(url);
                POPUP.on('move-to-commission-group', function (data) {
                    updateMemberCommissions(data);
                });
            }
        });

        // remove group
        $('#removeFromGroup').click(function () {
            var custIds = getSelectedMembers();
            if (custIds !== '') {
                var url = CORE.resolveUrl('~/CommissionSetting/MemberList/RemoveFromGroup', {
                    custIds: custIds
                });
                POPUP.openFrame(url);
                POPUP.on('remove-from-commission-group', function (data) {
                    updateMemberCommissions(data);
                });
            }
        });

        // update commission
        $('#updateCommission').click(function () {
            var custIds = getSelectedMembers();
            if (custIds !== '') {
                var url = CORE.resolveUrl('~/CommissionSetting/MemberList/UpdateMemberCommission', {
                    custIds: custIds
                });

                POPUP.openFrame(url);
                POPUP.on('update-member-commission', function (data) {
                    updateMemberCommissions(data);
                });
            }
        });

        $('#search-form').submit();
    };

    $this.reRenderAfterPaged = function (currentPage, itemsPerPage) {
        var params = $('#search-form').serializeObject();
        params.pageIndex = currentPage;
        params.pageSize = itemsPerPage;
        CORE.ajaxJSON({
            url: $('#search-form').attr('action'),
            data: params
        }, function (data) {
            var gridOptions = generateGridOptions(data.ProductColumns);
            gridOptions.pagerOption = $.extend({}, gridOptions.pagerOption, {
                pagerSelectCallBack: $this.reRenderAfterPaged
            });
            gridOptions.bodyRows = GRID.generateBodyRows(data.List, gridOptions);
            $('#grid').show().fanexGrid(gridOptions);
            changeButtonState();
        });
    };

    $this.checkAllItem = function (checkAll) {
        if ($(checkAll).hasClass("icon-uncheck-all")) {
            $(checkAll).removeClass("icon-uncheck-all").addClass("icon-checked-all");
            $(".cell-select").removeClass("icon-uncheck").addClass("icon-checked");
        }
        else {
            $(checkAll).removeClass("icon-checked-all").addClass("icon-uncheck-all");
            $(".cell-select").removeClass("icon-checked").addClass("icon-uncheck");
        }
        changeButtonState();
    };

    $this.selectClick = function (checkItem) {
        if ($(checkItem).hasClass("icon-uncheck")) {
            $(checkItem).removeClass("icon-uncheck").addClass("icon-checked");
        }
        else {
            $(checkItem).removeClass("icon-checked").addClass("icon-uncheck");
        }
        if ($('.icon-checked').length === $('.cell-select').length) {
            $('.icon-uncheck-all').removeClass("icon-uncheck-all").addClass("icon-checked-all");
        } else {
            $('.icon-checked-all').removeClass("icon-checked-all").addClass("icon-uncheck-all");
        }
        changeButtonState();
    };

    return $this;
} (COMMISSION_SETTING.MEMBER_LIST || {}, jQuery));