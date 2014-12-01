/*global CORE, LANGUAGE, POPUP, GRID, numeral*/
'use strict';

var COMMISSION_SETTING = COMMISSION_SETTING || {};
COMMISSION_SETTING.GROUP_LIST = (function ($this, $) {
    /* jshint -W101 */
    var formatNumber = function (value) {
        return numeral(value * 100).format('0.00');
    };

    var baseGridOptions = {
        columns: [
            { HeaderText: '#', Name: 'No', Width: 30, HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('GroupName'), Width: 200, Name: "GroupName", HeaderAlign: "center", AllowTruncate: true, CanResize: false },
            { HeaderText: LANGUAGE.getString('DefaultGroup'), Width: 50, Name: "IsDefault", HeaderAlign: "center", CellAlign: "center",
                Formatter: function (rowIndex, rowData, cell) {
                    return (cell.Value ? '<span class="icon-input-valid">&nbsp;</span>' : '');
                }
            },
            { HeaderText: LANGUAGE.getString('NoOfMember'), Width: 50, Name: "NumberOfCustomer", HeaderAlign: "center", CellAlign: "center",
                Formatter: function (rowIndex, rowData, cell) {
                    var commissionGroupId = rowData['GroupCommId'];
                    return '<span class="downline"><a href="#" onclick="COMMISSION_SETTING.GROUP_LIST.showMemberList({0})">{1}</a></span>'.format(commissionGroupId, cell.Value);
                }
            },
            { HeaderText: LANGUAGE.getString('Edit'), Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                Formatter: function (rowIndex, rowData) {
                    return '<i class="icon-customer-edit" onclick="COMMISSION_SETTING.GROUP_LIST.editGroup({0})">&nbsp;</i>&nbsp;<i class="icon-customer-delete" onclick="COMMISSION_SETTING.GROUP_LIST.deleteGroup({0}, {1})">&nbsp;</i>'.format(rowData['GroupCommId'], rowData['IsDefault']);
                }
            }
        ],
        pagerOption: {
            itemsPerPage: GRID.PageSizes[2],
            currentPage: 0,
            itemsPerPageArray: GRID.PageSizes
        },
        showPager: true,
        showNumberColumn: true,
        autoResizeColumn: true
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
        $.each(productColumns, function (index) {
            var productColumn = this;
            var gridColumn = generateGridColumn(productColumn);
            gridOptions.columns.splice(4 + index, 0, gridColumn);
        });
        return gridOptions;
    };
    /* jshint +W101 */

    var defaultGroupId = 0;

    $this.init = function () {
        $('#newGroup').click(function () {
            $this.newGroup();
        });
        $('#memberList').click(function () {
            $this.showMemberList();
        });

        $this.dataGridBinding();
    };

    $this.dataGridBinding = function () {
        CORE.ajaxJSON({
            url: document.URL
        }, function (data) {
            defaultGroupId = data.DefaultGroupId;
            var gridOptions = generateGridOptions(data.ProductColumns);
            gridOptions.bodyRows = GRID.generateBodyRows(data.List, gridOptions);
            $('#grid').show().fanexGrid(gridOptions);
        });
    };

    $this.newGroup = function () {
        POPUP.openFrame('~/CommissionSetting/GroupList/Add');
        POPUP.on('add-commission-group', function () {
            $this.dataGridBinding();
        });
    };

    $this.editGroup = function (groupCommId) {
        var url = CORE.resolveUrl('~/CommissionSetting/GroupList/Edit', {
            groupCommId: groupCommId,
            defaultGroupId: defaultGroupId
        });
        POPUP.openFrame(url);
        POPUP.on('update-commission-group', function () {
            $this.dataGridBinding();
        });
    };

    $this.deleteGroup = function (groupCommId, isDefault) {
        var confirmDeleteGroup = function () {
            var url = CORE.resolveUrl('~/CommissionSetting/GroupList/Delete', {
                groupCommId: groupCommId,
                isDefault: isDefault
            });
            POPUP.openFrame(url);
            POPUP.on('delete-commission-group', function () {
                $this.dataGridBinding();
            });
        };

        if (isDefault) {
            var buttons = [
            {
                label: LANGUAGE.getString('OK'),
                action: function () {
                    confirmDeleteGroup();
                }
            },
            {
                label: LANGUAGE.getString('Cancel'),
                action: function () {
                    POPUP.close();
                }
            }];

            POPUP.confirm({
                title: LANGUAGE.getString('Confirmation'),
                message: LANGUAGE.getString('ConfirmDeleteDefaultGroup'),
                buttons: buttons
            });
        }
        else {
            confirmDeleteGroup();
        }
    };

    $this.showMemberList = function (commissionGroupId) {
        var url;
        if (typeof commissionGroupId === 'undefined') {
            url = CORE.resolveUrl('~/CommissionSetting/MemberList');
        } else {
            url = CORE.resolveUrl('~/CommissionSetting/MemberList', {
                groupCommId: commissionGroupId
            });
        }
        document.location.href = url;
    };

    return $this;
} (COMMISSION_SETTING.GROUP_LIST || {}, jQuery));