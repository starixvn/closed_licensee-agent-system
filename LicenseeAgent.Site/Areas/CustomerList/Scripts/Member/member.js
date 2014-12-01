/*global CORE, LANGUAGE, POPUP, ENUMS, pageContext, context */
'use strict';

var CUSTOMER_LIST = CUSTOMER_LIST || {};
CUSTOMER_LIST.MEMBER_LIST = (function ($this, $) {
    /* jshint -W101 */
    var gridOptionsForCredit = {
        columns:
        [
            { HeaderText: "#", Width: 30, Name: "CustId", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Action'), Width: 35, Name: "Action", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData) {
                    return '<i class="icon-customer-edit" data-memberid="{0}" data-username="{1}"></i>'.format(rowData.CustId, rowData.UserName);
                }
            },
            { HeaderText: LANGUAGE.getString('Username'), Width: 100, Name: "UserName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 200 },
            { HeaderText: LANGUAGE.getString('Status'), Width: 65, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    return '<div class="cust-status" onclick="CUSTOMER_LIST.MEMBER_LIST.changeStatus(this, ' + rowData.CustId + ')">'
                        + (cell.Value === 1
                            ? '<div class="icon-status-open"><span>{0}</span></div>'.format(LANGUAGE.getString('Open'))
                            : '<div class="icon-status-closed"><span>{0}</span></div>'.format(LANGUAGE.getString('Closed')))
                        + '</div>';
                }
            },
            { HeaderText: LANGUAGE.getString('FullName'), Width: 200, Name: "CustomerName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 300 },
            { HeaderText: LANGUAGE.getString('Group'), Width: 50, Name: "GroupName", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('CreatedDate'), Width: 140, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", CanResize: false, FormatDateTime: CORE.FullDateFormat },
            { HeaderText: LANGUAGE.getString('LoginIp'), Width: 100, Name: "IpLogin", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    if (cell.Value == null) {
                        return '';
                    }
                    return '<a onclick=PAGE.showIP("{0}")>{1}</a>'.format(cell.Value, cell.Value);
                }
            }
        ],
        showPager: true,
        customPager: true,
        showNumberColumn: true,
        autoResizeColumn: true
    };

    var gridOptionsForCash = {
        columns:
        [
            { HeaderText: "#", Width: 30, Name: "CustId", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Action'), Width: 35, Name: "Action", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData) {
                    return '<i class="icon-customer-edit" data-memberid="{0}" data-username="{1}"></i>'.format(rowData.CustId, rowData.UserName);
                }
            },
            { HeaderText: LANGUAGE.getString('Username'), Width: 100, Name: "UserName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 200 },
            { HeaderText: LANGUAGE.getString('Status'), Width: 65, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    return '<div class="cust-status" onclick="CUSTOMER_LIST.MEMBER_LIST.changeStatus(this, ' + rowData.CustId + ')">'
                        + (cell.Value === 1
                            ? '<div class="icon-status-open"><span>{0}</span></div>'.format(LANGUAGE.getString('Open'))
                            : '<div class="icon-status-closed"><span>{0}</span></div>'.format(LANGUAGE.getString('Closed')))
                        + '</div>';
                }
            },
            { HeaderText: LANGUAGE.getString('AvailableFund'), Width: 80, Name: "AvailableFund", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true, MaxResizeWidth: 160, FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('PendingFund'), Width: 80, Name: "PendingFund", HeaderAlign: "center", CellAlign: "right", AllowTruncate: true, MaxResizeWidth: 160, FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString('CreatedDate'), Width: 140, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", CanResize: false, FormatDateTime: CORE.FullDateFormat },
            { HeaderText: LANGUAGE.getString('LoginIp'), Width: 100, Name: "IpLogin", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    if (cell.Value == null) {
                        return '';
                    }
                    return '<a onclick=PAGE.showIP("{0}")>{1}</a>'.format(cell.Value, cell.Value);
                }
            }
        ],
        showPager: true,
        customPager: true,
        showNumberColumn: true,
        autoResizeColumn: true
    };
    /* jshint +W101 */

    var gridOptions;
    if (pageContext.IsDeposit) {
        if (pageContext.IsAllowedMemberComm) {
            gridOptionsForCash.columns.splice(4, 0,
            { HeaderText: LANGUAGE.getString('CommGroup'), Width: 80, Name: "GroupName", HeaderAlign: "center", MaxResizeWidth: 160 });
        }
        gridOptions = gridOptionsForCash;
    }
    else {
        gridOptions = gridOptionsForCredit;
    }

    if (pageContext.IsAdmin) {
        gridOptions.columns.splice(2, 0,
            { HeaderText: LANGUAGE.getString('SystemId'), Width: 140, Name: "SystemId", Formatter:
                function (rowIndex, rowData, cell) {
                    return '<span title="' + rowData.SystemId + '">' + cell.Value + '</span>';
                }
            });
    }

    var createContextMenu = function () {
        var listAction = [
            {
                id: 'menuSportsbook',
                icon: 'icon-customer-canedit',
                text: LANGUAGE.getString('Sportsbook'),
                href: '#',
                css: 'popover-link',
                action: function (e) {
                    e.stopPropagation();
                    if (pageContext.IsAdmin) {
                        CUSTOMER_LIST.hidePopover();
                        $('#menuSportsbook').popover("show");
                    } else {
                        CUSTOMER_LIST.MEMBER_LIST.editSportsBook();
                    }
                }
            },
            {
                id: 'menuVirtualSports',
                icon: 'icon-customer-canedit',
                text: LANGUAGE.getString('VirtualSports'),
                href: '#',
                css: 'popover-link',
                action: function (e) {
                    e.stopPropagation();
                    if (pageContext.IsAdmin) {
                        CUSTOMER_LIST.hidePopover();
                        $('#menuVirtualSports').popover("show");
                    } else {
                        CUSTOMER_LIST.MEMBER_LIST.editVirtualSports();
                    }
                }
            },
            {
                id: 'menuKeno',
                icon: 'icon-customer-canedit',
                text: LANGUAGE.getString('Keno'),
                href: '#',
                css: 'popover-link',
                action: function (e) {
                    e.stopPropagation();
                    if (pageContext.IsAdmin) {
                        CUSTOMER_LIST.hidePopover();
                        $('#menuKeno').popover("show");
                    } else {
                        CUSTOMER_LIST.MEMBER_LIST.editKeno();
                    }
                }
            },
            {
                id: 'menuEditInfo',
                icon: 'icon-customer-canedit',
                text: LANGUAGE.getString('EditInfo'),
                href: '#',
                action: CUSTOMER_LIST.MEMBER_LIST.editInfo
            },
            {
                id: 'menuResetPassword',
                icon: 'icon-customer-canedit',
                text: LANGUAGE.getString('ResetPassword'),
                href: '#',
                action: CUSTOMER_LIST.MEMBER_LIST.resetPassword
            }
        ];

        context.attach('.icon-customer-edit', listAction);

        // create action context menu
        context.init({
            preventDoubleContext: false,
            beforeShow: beforeShowContext
        });
    };
    var beforeShowContext = function (target) {
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

        if (!pageContext.IsOfferedKeno) {
            $('#menuKeno').hide();
        }
    };

    var createPopover = function () {
        $('#menuSportsbook').popover({
            html: true,
            placement: 'right',
            trigger: 'manual',
            content: function () {
                var divId = "tmp-id-" + $.now();
                return CUSTOMER_LIST.MEMBER_LIST.getDirectUplines(divId, ENUMS.Product.SportsBook);
            }
        });

        $('#menuVirtualSports').popover({
            html: true,
            placement: 'right',
            trigger: 'manual',
            content: function () {
                var divId = "tmp-id-" + $.now();
                return CUSTOMER_LIST.MEMBER_LIST.getDirectUplines(divId, ENUMS.Product.VirtualSports);
            }
        });

        $('#menuKeno').popover({
            html: true,
            placement: 'right',
            trigger: 'manual',
            content: function () {
                var divId = "tmp-id-" + $.now();
                return CUSTOMER_LIST.MEMBER_LIST.getDirectUplines(divId, ENUMS.Product.Keno);
            }
        });

        $(document).on('click', 'html', function () {
            CUSTOMER_LIST.hidePopover();
        });
    };

    var openProductSetting = function (url) {
        POPUP.openFrame(url, {
            width: 1008,
            height: 680
        });
    };

    var editMemberId;
    var editUsername;

    $this.init = function () {
        CUSTOMER_LIST.init(gridOptions);
        createContextMenu();
        createPopover();
    };

    $this.getDirectUplines = function (divId, productId) {
        $.ajax({
            url: CORE.resolveUrl('~/CustomerList/Member/DirectUpLines'),
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
    };

    $this.editSportsBook = function () {
        var url = CORE.resolveUrl('~/ProductSetting/SportsBook/EditMember', {
            memberId: editMemberId
        });
        openProductSetting(url);
    };

    $this.editVirtualSports = function () {
        var url = CORE.resolveUrl('~/ProductSetting/VirtualSports/EditMember', {
            memberId: editMemberId
        });
        openProductSetting(url);
    };

    $this.editKeno = function () {
        var url = CORE.resolveUrl('~/ProductSetting/Keno/EditMember', {
            memberId: editMemberId
        });
        openProductSetting(url);
    };

    $this.resetPassword = function () {
        var url = CORE.resolveUrl('~/CustomerList/Member/ResetPassword', {
            memberId: editMemberId
        });
        POPUP.openFrame(url);
    };

    $this.editInfo = function () {
        var url = CORE.resolveUrl('~/CustomerList/Member/EditInfo', {
            memberId: editMemberId,
            userName: editUsername
        });
        POPUP.openFrame(url);
    };

    $this.changeStatus = function (target, custId) {
        if (!pageContext.IsDirectDownLine) {
            return;
        }
        var currentStatus = $(target).find('.icon-status-open').length > 0
                ? ENUMS.AccountStatus.Open
                : ENUMS.AccountStatus.Closed;
        var newStatus = currentStatus === ENUMS.AccountStatus.Closed
                ? ENUMS.AccountStatus.Open
                : ENUMS.AccountStatus.Closed;
        var message = newStatus === ENUMS.AccountStatus.Closed
                ? LANGUAGE.getString('ChangeStatusMember')
                : LANGUAGE.getString('ChangeStatusAgent');
        var title = LANGUAGE.getString('ChangeStatus');
        var buttons = [{
            label: LANGUAGE.getString('Yes'),
            action: function () {
                POPUP.close();
                CUSTOMER_LIST.MEMBER_LIST.updateStatus(target, custId, newStatus);
            }
        }, {
            label: LANGUAGE.getString('No'),
            action: function () {
                POPUP.close();
            }
        }];

        POPUP.confirm({
            title: title,
            message: message,
            buttons: buttons
        });
    };

    $this.updateStatus = function (target, custId, statusId) {
        CORE.ajaxJSON({
            url: CORE.resolveUrl('~/CustomerList/Member/ChangeStatus'),
            type: 'post',
            data: {
                memberid: custId,
                status: statusId,
                agentid: $("#AgentId").val()
            }
        }, function () {
            if (statusId === ENUMS.AccountStatus.Closed) {
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
            $('#' + custId).data('status', statusId);
        });
    };

    return $this;
} (CUSTOMER_LIST.MEMBER_LIST || {}, jQuery));