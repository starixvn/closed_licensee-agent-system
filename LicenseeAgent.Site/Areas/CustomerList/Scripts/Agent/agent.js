/*global core, language, popup, pageContext, context */

var customerList = customerList || {};
customerList.agent = (function ($) {
    'use strict';

    /* jshint -W101 */
    var gridOptions = {
        columns: [
            { HeaderText: "#", Width: 30, Name: "AgentId", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString('Action'), Width: 35, Name: "Action", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData) {
                    return '<i id="{0}" class="icon-customer-edit" data-agentid="{0}" data-agentlevel="{1}" data-status="{2}"></i>'.format(rowData.AgentId, rowData.AgentLevel, rowData.Status);
                }
            },
            { HeaderText: language.getString('Username'), Width: 100, Name: "UserName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 200 },
            { HeaderText: language.getString('Status'), Width: 65, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    return '<div class="cust-status" onclick="customerList.agent.changeStatus(this, ' + rowData.AgentId + ')">'
                        + (cell.Value === 1
                            ? '<div class="icon-status-open"><span>{0}</span></div>'.format(language.getString('Open'))
                            : '<div class="icon-status-closed"><span>{0}</span></div>'.format(language.getString('Closed')))
                        + '</div>';
                }
            },
            { HeaderText: language.getString('Downline'), Width: 60, Name: "NumberOfAgent", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    if (cell.Value === 0) {
                        return '<span class="downline">' + cell.Value + '</span>';
                    }
                    return '<span class="downline"><a href="javascript:void(0);" onclick="javascript:customerList.showAgent({0})">{1}</a></span>'.format(rowData.AgentId, cell.Value);
                }
            },
            { HeaderText: language.getString('DirectMember'), Width: 90, Name: "NumberOfMember", HeaderAlign: "center", CellAlign: "center", CanResize: false,
                Formatter: function (rowIndex, rowData, cell) {
                    if (cell.Value === 0) {
                        return '<span class="downline">' + cell.Value + '</span>';
                    }
                    return '<span class="downline"><a href="javascript:void(0);" onclick="javascript:customerList.showMember({0})">{1}</a></span>'.format(rowData.AgentId, cell.Value);
                }
            },
            { HeaderText: language.getString('FullName'), Width: 200, Name: "CustomerName", HeaderAlign: "center", AllowTruncate: true, MaxResizeWidth: 300 },
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

    var editAgentId = 0;
    var editAgentLevel = 0;
    return {
        init: function () {
            customerList.initCustomerList(gridOptions);
            customerList.agent.createContextMenu();
        },
        createContextMenu: function () {
            context.attach('.icon-customer-edit', [
                {
                    id: 'menuSportsbook',
                    icon: (pageContext.IsAdmin || pageContext.IsDirectDownLine
                        ? 'icon-customer-canedit'
                        : 'icon-customer-canview'),
                    text: language.getString('Sportsbook'),
                    href: '#',
                    css: 'popover-link',
                    action: customerList.agent.editSportsBook
                },
                {
                    id: 'menuVirtualSports',
                    icon: (pageContext.IsAdmin || pageContext.IsDirectDownLine
                        ? 'icon-customer-canedit'
                        : 'icon-customer-canview'),
                    text: language.getString('VirtualSports'),
                    href: '#',
                    css: 'popover-link',
                    action: customerList.agent.editVirtualSports
                },
                {
                    id: 'menuEditInfo',
                    icon: 'icon-customer-canedit',
                    text: language.getString('EditInfo'),
                    href: '#',
                    action: customerList.agent.editInfo
                },
                {
                    id: 'menuResetPassword',
                    icon: 'icon-customer-canedit',
                    text: language.getString('ResetPassword'),
                    href: '#',
                    action: customerList.agent.resetPassword
                },
                {
                    id: 'menuAddDownline',
                    icon: 'icon-customer-canedit',
                    text: language.getString('AddDownlineAgent'),
                    href: '#',
                    action: customerList.agent.addAgent
                }
            ]);

            // create action context menu
            context.init({
                preventDoubleContext: false,
                beforeShow: customerList.agent.beforeShowContext
            });
        },
        beforeShowContext: function (target) {
            editAgentId = parseInt($(target).data('agentid'), null);
            editAgentLevel = parseInt($(target).data('agentlevel'), null);

            if (pageContext.IsAdmin && parseInt($(target).data('status')) === core.enums.Status.Open) {
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
        },
        changeStatus: function (target, custId) {
            if (!pageContext.IsDirectDownLine) {
                return;
            }
            var currentStatus = $(target).find('.icon-status-open').length > 0 ? 1 : 0;
            var newStatus = currentStatus === 0 ? 1 : 0;
            var message = newStatus === core.enums.Status.Closed
                ? language.getString('ChangeStatusAgent')
                : language.getString('ChangeCloseToOpenAgent');
            var title = language.getString('ChangeStatus');
            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    popup.close();
                    customerList.agent.updateStatus(target, custId, newStatus);
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
                url: core.resolveUrl('~/CustomerList/Agent/ChangeStatus'),
                type: 'post',
                data: {
                    agentid: custId,
                    status: statusId
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
        editSportsBook: function () {
            var url = core.resolveUrl('~/ProductSetting/SportsBook/EditAgent', {
                agentId: editAgentId
            });
            popup.openFrame(url, {
                width: 1008,
                height: 680
            });
        },
        editVirtualSports: function () {
            var url = core.resolveUrl('~/ProductSetting/VirtualSports/EditAgent', {
                agentId: editAgentId
            });
            popup.openFrame(url, {
                width: 1008,
                height: 680
            });
        },
        editInfo: function () {
            var url = core.resolveUrl('~/CustomerList/Agent/EditInfo', {
                agentId: editAgentId
            });
            popup.openFrame(url);
        },
        addAgent: function () {
            var url = core.resolveUrl('~/ProductSetting/NewAgent', {
                agentId: editAgentId
            });
            popup.openFrame(url, {
                width: 1008,
                height: 680
            });
        },
        resetPassword: function () {
            var url = core.resolveUrl('~/CustomerList/Agent/ResetPassword', {
                agentId: editAgentId
            });
            popup.openFrame(url);
        }
    };
} (jQuery));