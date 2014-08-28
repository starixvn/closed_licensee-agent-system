/* global core, popup */

var admin = admin || {};
admin.maintenance = admin.maintenance || {};
admin.maintenance.syncmonitoring = (function ($) {
    'use strict';

    /* jshint -W101 */
    var checkboxHtml = '<input type="checkbox" id="chkbox_{0}" class="" {5} onchange="admin.maintenance.syncmonitoring.checkboxOnChange()" style="margin-left:20px;"';
    checkboxHtml += ' data-keyid="{0}" data-actionid="{1}" data-custid="{2}" data-userid="{3}" data-command="{4}"></input>';
    var buttonHtml = '<button id="btn_{0}" class="btn btn-primary" onclick="admin.maintenance.syncmonitoring.singleReInvoke({0}, {1}, {2}, {3}, \'{4}\')">Re-Invoke</button>';
    buttonHtml += '<br /><br /><button id="btnIgnore_{0}" class="btn btn-danger" onclick="admin.maintenance.syncmonitoring.singleIgnore({0})">Ignore</button>';
    buttonHtml += '<label id="lbl_{0}" class="hide" style="color: #FF0000;">Success</label>';
    var globalCheckAll = false;

    var gridOptions = {
        columns: [
            { HeaderText: '<input type="checkbox" id="chkbox_all" class="" onchange="admin.maintenance.syncmonitoring.checkboxAllOnChange()""></input>'
                + '<button id="btn_multreinvoke" class="btn btn-primary hide" onclick="admin.maintenance.syncmonitoring.multipleReInvoke()">Re-Invoke</button>',
                Width: 100, Name: "", HeaderAlign: "center", CellAlign: "center",
                Formatter: function (rowIndex, rowData) {
                    if (globalCheckAll === true) {
                        return checkboxHtml.format(rowData.KeyId, rowData.ActionId, rowData.CustId, rowData.UserId, rowData.Command, "checked");
                    }
                    else {
                        return checkboxHtml.format(rowData.KeyId, rowData.ActionId, rowData.CustId, rowData.UserId, rowData.Command, "");
                    }
                }
            },
            { HeaderText: "Log ID", Width: 36, Name: "KeyId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Log Date", Width: 72, Name: "LogDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: core.DATE_FORMAT_FULL },
            { HeaderText: "Action Name", Width: 120, Name: "ActionName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Input Params", Width: 280, Name: "InputParams", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Remarks", Width: 198, Name: "Remark", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Run Count", Width: 60, Name: "RunCount", HeaderAlign: "center", CellAlign: "center",
                Formatter: function (rowIndex, rowData) {
                    return '<label id="lblruncount_{0}">{1}</label>'.format(rowData.KeyId, rowData.RunCount);
                }
            },
            { HeaderText: "Action", Width: 80, Name: "", HeaderAlign: "center", CellAlign: "center",
                Formatter: function (rowIndex, rowData) {
                    return buttonHtml.format(rowData.KeyId, rowData.ActionId, rowData.CustId, rowData.UserId, rowData.Command);
                }
            }
        ],
        showPager: true,
        customPager: true,
        gridExpandHeight: 125,
        bodyRowHeight: 29
    };
    /* jshint +W101 */

    return {
        init: function () {
            $('form').submit(function (e) {
                globalCheckAll = false;
                admin.maintenance.syncmonitoring.reRenderAfterPaged(0, core.pageSizes[0]);
                e.preventDefault();
            });
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage) {
            var submitOptions = $('form').serializeObject();
            submitOptions.pageIndex = currentPage;
            submitOptions.pageSize = itemsPerPage;
            $.ajax({
                url: $('form').attr('action'),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    gridOptions.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: core.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.TotalRows,
                        pagerSelectCallBack: admin.maintenance.syncmonitoring.reRenderAfterPaged,
                        showDetail: true,
                        afterPagedFunction: function () {
                            $('#btn_multreinvoke').addClass('hide');
                            $('#chkbox_all').prop('checked', false);
                            $('#tblContent_monitoringGrid tbody input').each(function () {
                                this.checked = false;
                            });
                        }
                    };
                    gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                    $('#monitoringGrid').show().fanexGrid(gridOptions);

                    if (data.TotalRows === 0) {
                        $('#chkbox_all').addClass('hide');
                        $('.no-data-not-frozen').width(1020);
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        checkboxAllOnChange: function () {
            if ($('#chkbox_all').prop('checked')) {
                globalCheckAll = true;
                $('#tblContent_monitoringGrid tbody input').each(function () {
                    this.checked = true;
                });
                $('#btn_multreinvoke').removeClass('hide');
            }
            else {
                globalCheckAll = false;
                $('#tblContent_monitoringGrid tbody input').each(function () {
                    this.checked = false;
                });
                $('#btn_multreinvoke').addClass('hide');
            }
        },
        checkboxOnChange: function () {
            if ($('#tblContent_monitoringGrid tbody input:checked').length > 0) {
                $('#btn_multreinvoke').removeClass('hide');
                if ($('#tblContent_monitoringGrid tbody input').length === $('#tblContent_monitoringGrid tbody input:checked').length) {
                    $('#chkbox_all').prop('checked', true);
                    globalCheckAll = true;
                }
                else {
                    $('#chkbox_all').prop('checked', false);
                    globalCheckAll = false;
                }
            }
            else {
                $('#btn_multreinvoke').addClass('hide');
            }
        },
        refreshGrid: function () {
            var currentPage = gridOptions.pagerOption.currentPage;
            var currentPageSize = gridOptions.pagerOption.itemsPerPage;
            admin.maintenance.syncmonitoring.reRenderAfterPaged(currentPage, currentPageSize);
        },
        singleReInvoke: function (keyid) {
            var params = {
                KeyId: keyid
            };

            $.ajax({
                url: "SingleReInvoke",
                type: 'post',
                data: params,
                dataType: "json",
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        $('#chkbox_' + keyid).addClass('hide');
                        $('#btn_' + keyid).addClass('hide');
                        $('#btnIgnore_' + keyid).addClass('hide');
                        $('#lbl_' + keyid).removeClass('hide');
                    }
                    else {
                        var newRunCount = parseInt($('#lblruncount_' + keyid).text()) + 1;
                        $('#lblruncount_' + keyid).text(newRunCount);
                        var message = '<div>Re-Invoke failed for Log ID = {0}<br>{1}</div>'.format(keyid, data.ErrMsg);
                        popup.error({
                            message: message
                        });
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        singleIgnore: function (keyid) {
            var params = {
                KeyId: keyid
            };

            $.ajax({
                url: "SingleIgnore",
                type: 'post',
                data: params,
                dataType: "json",
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        $('#chkbox_' + keyid).addClass('hide');
                        $('#btn_' + keyid).addClass('hide');
                        $('#btnIgnore_' + keyid).addClass('hide');
                        $('#lbl_' + keyid).removeClass('hide');
                        $('#lbl_' + keyid).text('Ignored');
                    }
                    else {
                        var message = 'Ignore failed for Log ID = {0}.'.format(keyid);
                        popup.error({
                            message: message
                        });
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        multipleReInvoke: function () {
            $('#tblContent_monitoringGrid tbody input').each(function () {
                if ($(this).prop('checked') === true) {
                    admin.maintenance.syncmonitoring.singleReInvoke(
                        $(this).attr('data-keyid')
                    );
                }
            });
        }
    };
} (jQuery));