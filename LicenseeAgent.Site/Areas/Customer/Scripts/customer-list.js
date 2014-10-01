/*global core, language, popup, pageContext, gridOptions, context */
var customer = customer || {};
customer.customerList = (function ($) {
    'use strict';
    var statusObj = { All: -1, Close: 0, Open: 1 },
        editAgentId = 0,
        editAgentLevel = 0,
        editMemberId = 0,
        editUsername = "",
        sportsbook = "SportsBook",
        virtualSport = "VirtualSports";

    return {
        init: function () {
            $('.selectpicker').selectpicker();

            customer.customerList.createContextMenu();

            // breadcrum
            $('.breadcrumb a').click(function () {
                var agentId = $(this).data('agent-id'),
                    agentLevel = $(this).data('agent-level'),
                    agentName = $(this).text();
                customer.customerList.showAgent(agentId, agentName, agentLevel);
            });

            // search
            $('#search-form').submit(function (e) {
                customer.customerList.reRenderAfterPaged(0, core.pageSizes[2]);
                e.preventDefault();
            });

            // username suggestion
            $('#Username').typeahead({
                source: customer.customerList.getSuggestion,
                minLength: 2
            });

            // get customer list
            customer.customerList.reRenderAfterPaged(0, core.pageSizes[2]);
        },
        updateActionMenus: function (target) {
            if (!pageContext.IsAdmin
                || $(target).attr('data-status') === 0
                || pageContext.IsMember) {
                $('#menuAddDownline').hide();
            } else {
                $('#menuAddDownline').show();
            }

            if ((pageContext.IsAdmin || pageContext.IsDirectDownLine) && !(pageContext.IsDeposit && pageContext.IsMember)) {
                $('#menuEditInfo').show();
            } else {
                $('#menuEditInfo').hide();
            }

            if (!pageContext.IsOfferedVirtualSports) {
                $('#menuVirtualSports').hide();
            }

            if (pageContext.LegendType === 1
                && pageContext.AgentLevel === pageContext.NumberOfLevel) {
                $('#menuAddDownline').hide();
            }

            if (pageContext.IsMember) {
                editMemberId = parseInt($(target).data('memberid'), null);
                editUsername = $(target).data('username');
            } else {
                editAgentId = parseInt($(target).data('agentid'), null);
                editAgentLevel = parseInt($(target).data('agentlevel'), null);
            }
        },
        gridAfterBinding: function () {
            if (!pageContext.IsDirectDownLine) {
                $(".cust-status").addClass('cust-status-none');
            }
        },
        resizeBox: function () {
            $("#boxsearch").width($("#grid").width());
            $("#breadcrumb").width($("#grid").width());
        },
        showAgent: function (agentId, agentName, agentLevel, isMember) {
            $('#newAgentId').val(agentId);
            $('#AgentLevel').val(agentLevel);
            $('#AgentName').val(agentName);
            $('#IsMember').val(isMember);
            $("#ajax-form").submit();
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage) {
            if (gridOptions && gridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage : gridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
            }
            var params = {
                username: $.trim($("#Username").val()),
                status: $("#Status").val() !== -1 ? $.trim($("#Status").val()) : '',
                agentId: $("#AgentId").val(),
                agentLevel: $("#AgentLevel").val(),
                isMember: pageContext.IsMember,
                pageIndex: currentPage,
                pageSize: itemsPerPage
            };

            core.ajaxJSON({
                url: $('#search-form').attr('action'),
                data: params
            }, function (data) {
                if (data.ErrCode !== 0) {
                    alert(data.ErrMsg);
                } else {
                    gridOptions.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: core.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: customer.customerList.reRenderAfterPaged
                    };

                    gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                    $('#grid').show().fanexGrid(gridOptions);

                    customer.customerList.gridAfterBinding();
                    customer.customerList.resizeBox();
                }
            });
        },
        changeStatus: function (target, custId) {
            if (!pageContext.IsDirectDownLine) {
                return;
            }
            var currentStatus, newStatus;
            currentStatus = $(target).find('.icon-status-open').length > 0 ? 1 : 0;
            newStatus = currentStatus === 0 ? 1 : 0;
            customer.customerList.showAlert(target, custId, newStatus);
        },
        updateStatus: function (target, custId, statusId) {
            var params, url;
            if (pageContext.IsMember) {
                params = {
                    memberid: custId,
                    status: statusId,
                    agentid: $("#AgentId").val()
                };
            } else {
                params = {
                    agentid: custId,
                    status: statusId
                };
            }

            url = pageContext.IsMember ? "~/Customer/Member/ChangeStatus"
                : "~/Customer/Agent/ChangeStatus";
            $.ajax({
                url: core.resolveUrl(url),
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                        return;
                    }
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
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        checkAllItem: function (itemAll) {
            if ($(itemAll).hasClass("icon-uncheck-all")) {
                customer.customerList.checkAll();
            } else {
                customer.customerList.uncheckAll();
            }
        },
        checkAll: function () {
            $(".icon-uncheck-all").attr("class", "icon-checked-all");
            $(".icon-uncheck").attr("class", "icon-checked");
        },
        uncheckAll: function () {
            $(".icon-checked-all").attr("class", "icon-uncheck-all");
            $(".icon-checked").attr("class", "icon-uncheck");
        },
        selectClick: function (itemSelect) {
            if ($(itemSelect).hasClass("icon-uncheck")) {
                $(itemSelect).removeClass("icon-uncheck");
                $(itemSelect).addClass("icon-checked");
                if ($(".cell-select").length === $(".icon-checked").length) {
                    $(".icon-uncheck-all").attr("class", "icon-checked-all");
                }
            } else {
                $(itemSelect).removeClass("icon-checked");
                $(itemSelect).addClass("icon-uncheck");
                $(".icon-checked-all").attr("class", "icon-uncheck-all");
            }
        },
        getSuggestion: function (query, process) {
            return $.ajax({
                url: core.resolveUrl('~/CustomerList/CustomerSearchAutoComplete'),
                type: 'post',
                data: {
                    userName: query,
                    agentId: $("#AgentId").val(),
                    agentLevel: $("#AgentLevel").val()
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                        return;
                    }

                    return process(data.List);
                }
            });
        },
        showAlert: function (target, custId, statusId) {
            var message = '';
            if (statusId === statusObj.Close) {
                if (pageContext.IsMember) {
                    message = language.getString('ChangeStatusMember');
                } else {
                    message = language.getString('ChangeStatusAgent');
                }
            } else {
                if (pageContext.IsMember) {
                    message = language.getString('ChangeCloseToOpenMember');
                } else {
                    message = language.getString('ChangeCloseToOpenAgent');
                }
            }
            var title = language.getString('ChangeStatus');
            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    popup.close();
                    customer.customerList.updateStatus(target, custId, statusId);
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
        getIpInformation: function (ipAddress) {
            $.ajax({
                url: core.resolveUrl('~/Customer/CustomerList/GetIpInformation'),
                type: 'post',
                data: { IpAddress: ipAddress },
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data !== null) {
                        popup.open({
                            title: language.getString("IpInformation"),
                            message: data
                        });
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        editMember: function (memberId, status) {
            editMemberId = memberId;
            $.contextMenu({
                selector: '.cell-' + memberId,
                items: customer.customerList.getEditItems(status),
                trigger: "left",
                ignoreRightClick: true,
                autoHide: true
            });
        },
        getEditItems: function (status) {
            var editItems = {}, items = [], index;
            if (pageContext.IsMember) {
                items = [
                    {
                        name: language.getString("Sportsbook"),
                        callback: customer.customerList.editSportsbook
                    },
                    {
                        name: language.getString("EditInfo"),
                        callback: customer.customerList.editInfo
                    },
                    {
                        name: language.getString("ResetPassword"),
                        callback: customer.customerList.resetPassword
                    },
                    {
                        name: language.getString("Password"),
                        callback: customer.customerList.editSetting
                    }
                ];
            } else {
                items = [
                    {
                        name: language.getString("Sportsbook"),
                        callback: customer.customerList.editSportsbook
                    },
                    {
                        name: language.getString("EditInfo"),
                        callback: customer.customerList.editInfo
                    },
                    {
                        name: language.getString("ResetPassword"),
                        callback: customer.customerList.resetPassword
                    },
                    {
                        name: language.getString("Password"),
                        callback: customer.customerList.editSetting
                    }
                ];
            }

            if (pageContext.IsAdmin && !pageContext.IsMember && status) {
                items.push({
                    name: language.getString("AddAgent"),
                    callback: customer.customerList.addDownline
                });
            }
            if (pageContext.IsDirectDownLine) {
                for (index = 0; index < items.length; index++) {
                    editItems[index] = {
                        name: "<span class='action-content'>" +
                            "<i class='icon-customer-canedit'></i>" +
                            items[index].name + "</span>",
                        callback: items[index].callback
                    };
                }
            } else {
                for (index = 0; index < items.length; index++) {
                    editItems[index] = {
                        name: "<span class='action-content'>" +
                            "<i class='icon-customer-canview'></i>" +
                            items[index].name + "</span>",
                        callback: items[index].callback
                    };
                }
            }

            return editItems;
        },
        editSportsbook: function () {
            var url = "ProductSetting/SportsBook/EditAgent?agentId={0}".format(editAgentId, editAgentLevel);
            popup.openFrame(url, {
                width: 1008,
                height: 680
            });
        },
        editVirttualSports: function () {
            var url = "ProductSetting/VirtualSports/EditAgent?agentId={0}".format(editAgentId, editAgentLevel);
            popup.openFrame(url, {
                width: 1008,
                height: 680
            });
        },
        editSetting: function () {
            alert('test');
        },
        editInfo: function () {
            var url;
            if (pageContext.IsMember) {
                url = "Customer/Member/EditInfo?memberId={0}&username={1}".format(editMemberId, editUsername);
            } else {
                url = "Customer/Agent/EditInfo?agentId={0}".format(editAgentId);
            }
            popup.openFrame(url);
        },
        addDownline: function () {
            var url = "ProductSetting/SportsBook/InsertCustomer?agentId={0}"
                .format(editAgentId);
            popup.openFrame(url, {
                width: 1008,
                height: 680
            });
        },
        resetPassword: function () {
            var url;
            if (pageContext.IsMember) {
                url = "Customer/Member/ResetPassword?memberId={0}".format(editMemberId);
            } else {
                url = "Customer/Agent/ResetPassword?agentId={0}".format(editAgentId);
            }
            popup.openFrame(url);
        },
        hidePopover: function () {
            $('.popover-link').each(function () {
                $(this).popover('hide');
            });
        },
        uplineAgents: function (link, divId, product) {
            $.ajax({
                url: core.resolveUrl('~/Customer/Agent/UplineAgents'),
                data: 'custId=' + editMemberId + '&product=' + product,
                success: function (response) {
                    $('#' + divId).html(response);
                }
            });
            return '<div id="' + divId + '">Loading...</div>';
        },
        productSetting: function (productLink) {
            popup.openFrame($(productLink).data('url'), {
                width: 1008,
                height: 680
            });
        },
        createContextMenu: function () {
            var listAction = [
                {
                    id: 'menuSportsbook',
                    icon: (pageContext.IsAdmin ||
                        pageContext.IsMember ||
                        pageContext.IsDirectDownLine ?
                        'icon-customer-canedit'
                        : 'icon-customer-canview'),
                    text: language.getString('Sportsbook'),
                    href: '#',
                    css: 'popover-link',
                    action: function (e) {
                        if (pageContext.IsMember) {
                            e.stopPropagation();
                            customer.customerList.hidePopover();
                            $('#menuSportsbook').popover("show");
                        }
                        else {
                            customer.customerList.editSportsbook;
                        }
                    }
                },
                {
                    id: 'menuVirtualSports',
                    icon: (pageContext.IsAdmin ||
                        pageContext.IsMember ||
                        pageContext.IsDirectDownLine ?
                        'icon-customer-canedit'
                        : 'icon-customer-canview'),
                    text: language.getString('VirtualSports'),
                    href: '#',
                    css: 'popover-link',
                    action: function (e) {
                        if (pageContext.IsMember) {
                            e.stopPropagation();
                            customer.customerList.hidePopover();
                            $('#menuVirtualSports').popover("show");
                        }
                        else {
                            customer.customerList.editVirttualSports;
                        }
                    }
                },
                {
                    id: 'menuEditInfo',
                    icon: 'icon-customer-canedit',
                    text: language.getString('EditInfo'),
                    href: '#',
                    action: customer.customerList.editInfo
                },
                {
                    id: 'menuResetPassword',
                    icon: 'icon-customer-canedit',
                    text: language.getString('ResetPassword'),
                    href: '#',
                    action: customer.customerList.resetPassword
                },
                {
                    id: 'menuAddDownline',
                    icon: 'icon-customer-canedit',
                    text: language.getString('AddAgent'),
                    href: '#',
                    action: customer.customerList.addDownline
                }
            ];

            context.attach('.icon-customer-edit', listAction);

            if (pageContext.IsMember) {
                $('#menuVirtualSports').popover({
                    html: true,
                    placement: 'right',
                    trigger: 'manual',
                    content: function () {
                        var divId = "tmp-id-" + $.now();
                        return customer.customerList.uplineAgents($(this).attr('href'), divId, virtualSport);
                    }
                });

                $('#menuSportsbook').popover({
                    html: true,
                    placement: 'right',
                    trigger: 'manual',
                    content: function () {
                        var divId = "tmp-id-" + $.now();
                        return customer.customerList.uplineAgents($(this).attr('href'), divId, sportsbook);
                    }
                });

                $(document).on('click', 'html', function () {
                    customer.customerList.hidePopover();
                });
            }

            context.attach('#select-dropdown', [
                {
                    text: language.getString('All'),
                    href: '#',
                    action: customer.customerList.checkAll
                },
                {
                    text: language.getString('None'),
                    href: '#',
                    action: customer.customerList.uncheckAll
                }
            ]);
            // create action context menu
            context.init({
                preventDoubleContext: false,
                beforeShow: customer.customerList.updateActionMenus
            });           
        }
    };
} (jQuery));
