/*global CORE, LANGUAGE, POPUP, ENUMS, pageContext, context */
'use strict';

var CUSTOMER_LIST = CUSTOMER_LIST || {};
CUSTOMER_LIST.AGENT_LIST = (function ($this, $) {
    /* jshint -W101 */
    var gridOptions = {
        columns: [
            { HeaderText: "#", Width: 30, Name: "AgentId", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Action'), Width: 35, Name: "Action", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData) {
                    return '<i id="{0}" class="icon-customer-edit" data-agentid="{0}" data-agentlevel="{1}" data-status="{2}"></i>'.format(rowData.AgentId, rowData.AgentLevel, rowData.Status);
                }
            },
            { HeaderText: LANGUAGE.getString('Username'), Width: 100, Name: "UserName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 200 },
            { HeaderText: LANGUAGE.getString('Status'), Width: 65, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    return '<div class="cust-status" onclick="CUSTOMER_LIST.AGENT_LIST.changeStatus(this, ' + rowData.AgentId + ')">'
                        + (cell.Value === 1
                            ? '<div class="icon-status-open"><span>{0}</span></div>'.format(LANGUAGE.getString('Open'))
                            : '<div class="icon-status-closed"><span>{0}</span></div>'.format(LANGUAGE.getString('Closed')))
                        + '</div>';
                }
            },
            { HeaderText: LANGUAGE.getString('Downline'), Width: 60, Name: "NumberOfAgent", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    if (cell.Value === 0) {
                        return '<span class="downline">' + cell.Value + '</span>';
                    }
                    return '<span class="downline"><a href="javascript:void(0);" onclick="javascript:CUSTOMER_LIST.showAgent({0})">{1}</a></span>'.format(rowData.AgentId, cell.Value);
                }
            },
            { HeaderText: LANGUAGE.getString('DirectMember'), Width: 90, Name: "NumberOfMember", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    if (cell.Value === 0) {
                        return '<span class="downline">' + cell.Value + '</span>';
                    }
                    return '<span class="downline"><a href="javascript:void(0);" onclick="javascript:CUSTOMER_LIST.showMember({0})">{1}</a></span>'.format(rowData.AgentId, cell.Value);
                }
            },
            { HeaderText: LANGUAGE.getString('FullName'), Width: 200, Name: "CustomerName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 300 },
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

    var openProductSetting = function (url) {
        POPUP.openFrame(url, {
            width: 1008,
            height: 680
        });
    };

    var editAgentId = 0;
    var editAgentLevel = 0;
    var createContextMenu = function () {
        context.attach('.icon-customer-edit', [
            {
                id: 'menuSportsbook',
                icon: (pageContext.IsAdmin || pageContext.IsDirectDownLine
                    ? 'icon-customer-canedit'
                    : 'icon-customer-canview'),
                text: LANGUAGE.getString('Sportsbook'),
                href: '#',
                css: 'popover-link',
                action: editSportsBook
            },
            {
                id: 'menuVirtualSports',
                icon: (pageContext.IsAdmin || pageContext.IsDirectDownLine
                    ? 'icon-customer-canedit'
                    : 'icon-customer-canview'),
                text: LANGUAGE.getString('VirtualSports'),
                href: '#',
                css: 'popover-link',
                action: editVirtualSports
            },
            {
                id: 'menuKeno',
                icon: (pageContext.IsAdmin || pageContext.IsDirectDownLine
                    ? 'icon-customer-canedit'
                    : 'icon-customer-canview'),
                text: LANGUAGE.getString('Keno'),
                href: '#',
                css: 'popover-link',
                action: editKeno
            },
            {
                id: 'menuEditInfo',
                icon: 'icon-customer-canedit',
                text: LANGUAGE.getString('EditInfo'),
                href: '#',
                action: editInfo
            },
            {
                id: 'menuResetPassword',
                icon: 'icon-customer-canedit',
                text: LANGUAGE.getString('ResetPassword'),
                href: '#',
                action: resetPassword
            },
            {
                id: 'menuAddDownline',
                icon: 'icon-customer-canedit',
                text: LANGUAGE.getString('AddDownlineAgent'),
                href: '#',
                action: addAgent
            },
            {
                id: 'menuAddMember',
                icon: 'icon-customer-canedit',
                text: "Add Member",
                href: '#',
                action: addMember
            }
        ]);

        // create action context menu
        context.init({
            preventDoubleContext: false,
            beforeShow: beforeShowContext
        });
    };

    var beforeShowContext = function (target) {
        editAgentId = parseInt($(target).data('agentid'), null);
        editAgentLevel = parseInt($(target).data('agentlevel'), null);

        if (pageContext.IsAdmin
                && parseInt($(target).data('status')) === ENUMS.AccountStatus.Open
                && pageContext.AgentLevel < pageContext.NumberOfLevel) {
            $('#menuAddDownline').show();
        } else {
            $('#menuAddDownline').hide();
        }

        if (pageContext.IsAdmin || pageContext.IsDirectDownLine) {
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

    $this.init = function () {
        CUSTOMER_LIST.init(gridOptions);
        createContextMenu();
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
                ? LANGUAGE.getString('ChangeStatusAgent')
                : LANGUAGE.getString('ChangeCloseToOpenAgent');
        var title = LANGUAGE.getString('ChangeStatus');
        var buttons = [{
            label: LANGUAGE.getString('Yes'),
            action: function () {
                POPUP.close();
                CUSTOMER_LIST.AGENT_LIST.updateStatus(target, custId, newStatus);
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
            url: CORE.resolveUrl('~/CustomerList/Agent/ChangeStatus'),
            type: 'post',
            data: {
                agentid: custId,
                status: statusId
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

    var editSportsBook = function () {
        var url = CORE.resolveUrl('~/ProductSetting/SportsBook/EditAgent', {
            agentId: editAgentId
        });
        openProductSetting(url);
    };

    var editVirtualSports = function () {
        var url = CORE.resolveUrl('~/ProductSetting/VirtualSports/EditAgent', {
            agentId: editAgentId
        });
        openProductSetting(url);
    };

    var editKeno = function () {
        var url = CORE.resolveUrl('~/ProductSetting/Keno/EditAgent', {
            agentId: editAgentId
        });
        openProductSetting(url);
    };

    var editInfo = function () {
        var url = CORE.resolveUrl('~/CustomerList/Agent/EditInfo', {
            agentId: editAgentId
        });
        POPUP.openFrame(url);
    };

    var addAgent = function () {
        var url = CORE.resolveUrl('~/ProductSetting/NewAgent', {
            agentId: editAgentId
        });
        openProductSetting(url);
    };

    var addMember = function () {
        var url = CORE.resolveUrl('~/ProductSetting/SportsBook/InsertCustomer', {
            agentId: editAgentId
        });
        openProductSetting(url);
    };

    var resetPassword = function () {
        var url = CORE.resolveUrl('~/CustomerList/Agent/ResetPassword', {
            agentId: editAgentId
        });
        POPUP.openFrame(url);
    };

    return $this;
} (CUSTOMER_LIST.AGENT_LIST || {}, jQuery));