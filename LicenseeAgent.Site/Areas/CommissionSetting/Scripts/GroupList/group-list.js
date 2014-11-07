/*global language, core, popup, numeral*/
'use strict';

var commissionSetting = commissionSetting || {};
commissionSetting.groupList = (function ($this, $) {
    /* jshint -W101 */
    var formatNumber = function (value) {
        return numeral(value * 100).format('0.00');
    };

    var baseGridOptions = {
        columns: [
            { HeaderText: '#', Name: 'No', Width: 30, HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString('GroupName'), Width: 200, Name: "GroupName", HeaderAlign: "center", AllowTruncate: true, CanResize: false },
            { HeaderText: language.getString('DefaultGroup'), Width: 50, Name: "IsDefault", HeaderAlign: "center", CellAlign: "center",
                Formatter: function (rowIndex, rowData, cell) {
                    return (cell.Value ? '<span class="icon-input-valid">&nbsp;</span>' : '');
                }
            },
            { HeaderText: language.getString('NoOfMember'), Width: 50, Name: "NumberOfCustomer", HeaderAlign: "center", CellAlign: "center",
                Formatter: function (rowIndex, rowData, cell) {
                    var commissionGroupId = rowData['GroupCommId'];
                    return '<span class="downline"><a href="#" onclick="commissionSetting.groupList.showMemberList({0})">{1}</a></span>'.format(commissionGroupId, cell.Value);
                }
            },
            { HeaderText: language.getString('Edit'), Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                Formatter: function (rowIndex, rowData) {
                    return '<i class="icon-customer-edit" onclick="commissionSetting.groupList.editGroup({0})">&nbsp;</i>&nbsp;<i class="icon-customer-delete" onclick="commissionSetting.groupList.deleteGroup({0}, {1})">&nbsp;</i>'.format(rowData['GroupCommId'], rowData['IsDefault']);
                }
            }
        ],
        pagerOption: {
            itemsPerPage: core.pageSizes[2],
            currentPage: 0,
            itemsPerPageArray: core.pageSizes
        },
        showPager: true,
        indexNumberColumn: 0,
        bodyRowHeight: 29,
        autoResizeColumn: true
    };

    var generateGridColumn = function (productColumn) {
        return { HeaderText: language.getString(productColumn.HeaderResource), Width: 50, Name: productColumn.ColumnName, FormatNumber: "0,0.00", HeaderAlign: "center", CellAlign: "center",
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
        core.ajaxJSON({
            url: document.URL
        }, function (data) {
            defaultGroupId = data.DefaultGroupId;
            var gridOptions = generateGridOptions(data.ProductColumns);
            gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
            $('#grid').show().fanexGrid(gridOptions);
        });
    };

    $this.newGroup = function () {
        popup.openFrame('~/CommissionSetting/GroupList/Add');
        popup.on('add-commission-group', function () {
            $this.dataGridBinding();
        });
    };

    $this.editGroup = function (groupCommId) {
        var url = core.resolveUrl('~/CommissionSetting/GroupList/Edit', {
            groupCommId: groupCommId,
            defaultGroupId: defaultGroupId
        });
        popup.openFrame(url);
        popup.on('update-commission-group', function () {
            $this.dataGridBinding();
        });
    };

    $this.deleteGroup = function (groupCommId, isDefault) {
        var confirmDeleteGroup = function () {
            var url = core.resolveUrl('~/CommissionSetting/GroupList/Delete', {
                groupCommId: groupCommId,
                isDefault: isDefault
            });
            popup.openFrame(url);
            popup.on('delete-commission-group', function () {
                $this.dataGridBinding();
            });
        };

        if (isDefault) {
            var buttons = [
            {
                label: language.getString('OK'),
                action: function () {
                    confirmDeleteGroup();
                }
            },
            {
                label: language.getString('Cancel'),
                action: function () {
                    popup.close();
                }
            }];

            popup.confirm({
                title: language.getString('Confirmation'),
                message: language.getString('ConfirmDeleteDefaultGroup'),
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
            url = core.resolveUrl('~/CommissionSetting/MemberList');
        } else {
            url = core.resolveUrl('~/CommissionSetting/MemberList', {
                groupCommId: commissionGroupId
            });
        }
        document.location.href = url;
    };

    return $this;
} (commissionSetting.groupList || {}, jQuery));