/*global core, language, popup, pageContext, context */
var customerList = customerList || {};
customerList.member = (function ($) {
    'use strict';

    /* jshint -W101 */
    var gridOptionsForCredit = {
        columns:
        [
            { HeaderText: "#", Width: 30, Name: "CustId", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString('Action'), Width: 35, Name: "Action", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData) {
                    return '<i class="icon-customer-edit" data-memberid="{0}" data-username="{1}"></i>'.format(rowData.CustId, rowData.UserName);
                }
            },
            { HeaderText: language.getString('Username'), Width: 100, Name: "UserName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 200 },
            { HeaderText: language.getString('Status'), Width: 65, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    return '<div class="cust-status" onclick="customerList.member.changeStatus(this, ' + rowData.CustId + ')">'
                        + (cell.Value === 1
                            ? '<div class="icon-status-open"><span>{0}</span></div>'.format(language.getString('Open'))
                            : '<div class="icon-status-closed"><span>{0}</span></div>'.format(language.getString('Closed')))
                        + '</div>';
                }
            },
            { HeaderText: language.getString('FullName'), Width: 200, Name: "CustomerName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 300 },
            { HeaderText: language.getString('Group'), Width: 50, Name: "GroupName", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString('CreatedDate'), Width: 140, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", CanResize: false, FormatDateTime: core.DATE_FORMAT_FULL },
            { HeaderText: language.getString('LoginIp'), Width: 100, Name: "IpLogin", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    if (cell.Value == null) {
                        return '';
                    }
                    return '<a onclick=core.common.showIP("{0}")>{1}</a>'.format(cell.Value, cell.Value);
                }
            }
        ],
        showPager: true,
        customPager: true,
        gridExpandHeight: 125,
        indexNumberColumn: 0,
        bodyRowHeight: 29,
        autoResizeColumn: true
    };

    var gridOptionsForCash = {
        columns:
        [
            { HeaderText: "#", Width: 30, Name: "CustId", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString('Action'), Width: 35, Name: "Action", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData) {
                    return '<i class="icon-customer-edit" data-memberid="{0}" data-username="{1}"></i>'.format(rowData.CustId, rowData.UserName);
                }
            },
            { HeaderText: language.getString('Username'), Width: 100, Name: "UserName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 200 },
            { HeaderText: language.getString('Status'), Width: 65, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    return '<div class="cust-status" onclick="customerList.member.changeStatus(this, ' + rowData.CustId + ')">'
                        + (cell.Value === 1
                            ? '<div class="icon-status-open"><span>{0}</span></div>'.format(language.getString('Open'))
                            : '<div class="icon-status-closed"><span>{0}</span></div>'.format(language.getString('Closed')))
                        + '</div>';
                }
            },
            { HeaderText: language.getString('AvailableFund'), Width: 80, Name: "AvailableFund", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true, MaxResizeWidth: 160, FormatNumber: "0,0.00" },
            { HeaderText: language.getString('PendingFund'), Width: 80, Name: "PendingFund", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true, MaxResizeWidth: 160, FormatNumber: "0,0.00" },
            { HeaderText: language.getString('CreatedDate'), Width: 140, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", CanResize: false, FormatDateTime: core.DATE_FORMAT_FULL },
            { HeaderText: language.getString('LoginIp'), Width: 100, Name: "IpLogin", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    if (cell.Value == null) {
                        return '';
                    }
                    return '<a onclick=core.common.showIP("{0}")>{1}</a>'.format(cell.Value, cell.Value);
                }
            }
        ],
        showPager: true,
        customPager: true,
        gridExpandHeight: 125,
        indexNumberColumn: 0,
        bodyRowHeight: 29,
        autoResizeColumn: true
    };
    /* jshint +W101 */

    var gridOptions;
    if (pageContext.IsDeposit) {
        if (pageContext.IsAllowedMemberComm) {
            gridOptionsForCash.columns.splice(4, 0,
            { HeaderText: "@Las.CommGroup", Width: 80, Name: "GroupName", HeaderAlign: "center", MaxResizeWidth: 160 });
        }
        gridOptions = gridOptionsForCash;
    }
    else {
        gridOptions = gridOptionsForCredit;
    }

    if (pageContext.IsAdmin) {
        gridOptions.columns.splice(2, 0,
            { HeaderText: language.getString('SystemId'), Width: 140, Name: "SystemId", Formatter:
                function (rowIndex, rowData, cell) {
                    return '<span title="' + rowData.SystemId + '">' + cell.Value + '</span>';
                }
            });
    }

    var editMemberId;
    var editUsername;
    return {
        init: function () {
            customerList.initCustomerList(gridOptions);
            customerList.member.createContextMenu();
            customerList.member.createPopover();
        },
        createContextMenu: function () {
            var listAction = [
                {
                    id: 'menuSportsbook',
                    icon: 'icon-customer-canedit',
                    text: language.getString('Sportsbook'),
                    href: '#',
                    css: 'popover-link',
                    action: function (e) {
                        e.stopPropagation();
                        if (pageContext.IsAdmin) {
                            customerList.hidePopover();
                            $('#menuSportsbook').popover("show");
                        } else {
                            customerList.member.editSportsBook();
                        }
                    }
                },
                {
                    id: 'menuVirtualSports',
                    icon: 'icon-customer-canedit',
                    text: language.getString('VirtualSports'),
                    href: '#',
                    css: 'popover-link',
                    action: function (e) {
                        e.stopPropagation();
                        if (pageContext.IsAdmin) {
                            customerList.hidePopover();
                            $('#menuVirtualSports').popover("show");
                        } else {
                            customerList.member.editVirtualSports();
                        }
                    }
                },
                {
                    id: 'menuEditInfo',
                    icon: 'icon-customer-canedit',
                    text: language.getString('EditInfo'),
                    href: '#',
                    action: customerList.member.editInfo
                },
                {
                    id: 'menuResetPassword',
                    icon: 'icon-customer-canedit',
                    text: language.getString('ResetPassword'),
                    href: '#',
                    action: customerList.member.resetPassword
                }
            ];

            context.attach('.icon-customer-edit', listAction);

            // create action context menu
            context.init({
                preventDoubleContext: false,
                beforeShow: customerList.member.beforeShowContext
            });
        },
        beforeShowContext: function (target) {
            editMemberId = parseInt($(target).data('memberid'), null);
            editUsername = $(target).data('username');

            if ((pageContext.IsAdmin || pageContext.IsDirectDownLine) && !pageContext.IsDeposit) {
                $('#menuEditInfo').show();
            } else {
                $('#menuEditInfo').hide();
            }

            if (!pageContext.IsOfferedVirtualSports) {
                $('#menuVirtualSports').hide();
            }
        },
        createPopover: function () {
            $('#menuSportsbook').popover({
                html: true,
                placement: 'right',
                trigger: 'manual',
                content: function () {
                    var divId = "tmp-id-" + $.now();
                    return customerList.member.getDirectUplines(divId, core.enums.Product.SportsBook);
                }
            });

            $('#menuVirtualSports').popover({
                html: true,
                placement: 'right',
                trigger: 'manual',
                content: function () {
                    var divId = "tmp-id-" + $.now();
                    return customerList.member.getDirectUplines(divId, core.enums.Product.VirtualSports);
                }
            });

            $(document).on('click', 'html', function () {
                customerList.hidePopover();
            });
        },
        getDirectUplines: function (divId, productId) {
            $.ajax({
                url: core.resolveUrl('~/CustomerList/Member/DirectUpLines'),
                type: 'post',
                data: {
                    custId: editMemberId,
                    productId: productId
                },
                success: function (response) {
                    $('#' + divId).html(response);
                }
            });
            return '<div id="' + divId + '">Loading...</div>';
        },
        editSportsBook: function () {
            var url = core.resolveUrl('~/ProductSetting/SportsBook/EditMember', {
                memberId: editMemberId
            });
            popup.openFrame(url, {
                width: 1008,
                height: 680
            });
        },
        editVirtualSports: function () {
            var url = core.resolveUrl('~/ProductSetting/VirtualSports/EditMember', {
                memberId: editMemberId
            });
            popup.openFrame(url, {
                width: 1008,
                height: 680
            });
        },
        resetPassword: function () {
            var url = core.resolveUrl('~/CustomerList/Member/ResetPassword', {
                memberId: editMemberId
            });
            popup.openFrame(url);
        },
        editInfo: function () {
            var url = core.resolveUrl('~/CustomerList/Member/EditInfo', {
                memberId: editMemberId,
                userName: editUsername
            });
            popup.openFrame(url);
        },
        changeStatus: function (target, custId) {
            if (!pageContext.IsDirectDownLine) {
                return;
            }
            var currentStatus = $(target).find('.icon-status-open').length > 0 ? 1 : 0;
            var newStatus = currentStatus === 0 ? 1 : 0;
            var message = newStatus === core.enums.AccountStatus.Closed
                ? language.getString('ChangeStatusMember')
                : language.getString('ChangeStatusAgent');
            var title = language.getString('ChangeStatus');
            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    popup.close();
                    customerList.member.updateStatus(target, custId, newStatus);
                }
            }, {
                label: language.getString('No'),
                action: function () {
                    popup.close();
                }
            }];

            popup.confirm({
                title: title,
                message: message,
                buttons: buttons
            });
        },
        updateStatus: function (target, custId, statusId) {
            core.ajaxJSON({
                url: core.resolveUrl('~/CustomerList/Member/ChangeStatus'),
                type: 'post',
                data: {
                    memberid: custId,
                    status: statusId,
                    agentid: $("#AgentId").val()
                }
            }, function () {
                if (statusId === 0) {
                    $(target).find('.icon-status-open')
                        .addClass('icon-status-closed')
                        .removeClass('icon-status-open')
                        .find('span')
                        .text('Closed');
                } else {
                    $(target).find('.icon-status-closed')
                        .addClass('icon-status-open')
                        .removeClass('icon-status-closed')
                        .find('span')
                        .text('Open');
                }
                $('#' + custId).attr('data-status', statusId);
            });
        },
        openProductSetting: function (url) {
            popup.openFrame(url, {
                width: 1008,
                height: 680
            });
        }
    };
} (jQuery));