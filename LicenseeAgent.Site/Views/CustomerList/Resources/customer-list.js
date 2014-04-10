/*global core, language, popup, pageContext, msg, gridOptions, context */
var customerList = (function ($) {
    'use strict';
    var statusObj = { All: -1, Close: 0, Open: 1 },
        editAgentId = 0,
        editAgentLevel = 0,
        editMemberId = 0,
        editUsername = "";

    return {
        init: function () {
            $('.selectpicker').selectpicker();

            // create action context menu
            context.init({
                preventDoubleContext: false,
                beforeShow: customerList.updateActionMenus
            });

            context.attach('.icon-customer-edit', [
                {
                    id: 'menuSportsbook',
                    icon: (pageContext.IsAdmin ||
                        pageContext.IsLastLevel ||
                        pageContext.IsDirectDownline ?
                        'icon-customer-canedit'
                        : 'icon-customer-canview'),
                    text: language.getString('Sportsbook'),
                    href: '#',
                    action: customerList.editSportsbook
                },
                {
                    id: 'menuEditInfo',
                    icon: 'icon-customer-canedit',
                    text: language.getString('EditInfo'),
                    href: '#',
                    action: customerList.editInfo
                },
                {
                    id: 'menuResetPassword',
                    icon: 'icon-customer-canedit',
                    text: language.getString('ResetPassword'),
                    href: '#',
                    action: customerList.resetPassword
                },
                {
                    id: 'menuAddDownline',
                    icon: 'icon-customer-canedit',
                    text: language.getString('AddDownline'),
                    href: '#',
                    action: customerList.addDownline
                }
            ]);

            // create select context menu
            context.init({
                preventDoubleContext: false
            });
            context.attach('#select-dropdown', [
                {
                    text: language.getString('All'),
                    href: '#',
                    action: customerList.checkAll
                },
                {
                    text: language.getString('None'),
                    href: '#',
                    action: customerList.uncheckAll
                }
            ]);

            // breadcrum
            $('.breadcrumb a').click(function () {
                var agentId = $(this).data('agent-id'),
                    agentLevel = $(this).data('agent-level'),
                    agentName = $(this).text();
                customerList.showAgent(agentId, agentName, agentLevel);
            });

            // search
            $('#search-form').submit(function (e) {
                customerList.reRenderAfterPaged(0, core.pageSizes[2]);
                e.preventDefault();
            });

            // username suggestion
            $('#Username').typeahead({
                source: customerList.getSuggestion,
                minLength: 2
            });

            // get customer list
            customerList.reRenderAfterPaged(0, core.pageSizes[2]);
        },
        initInstruction: function () {
            $('.instruction-container').each(function () {
                $(this).click(function () {
                    customerList.getIpInformation($(this).html());
                });
            });
        },
        updateActionMenus: function (target) {
            if (!pageContext.IsAdmin ||
                $(target).attr('data-status') === 0 ||
                pageContext.IsLastLevel) {
                $('#menuAddDownline').hide();
            } else {
                $('#menuAddDownline').show();
            }

            if (pageContext.IsAdmin || pageContext.IsDirectDownline) {
                $('#menuEditInfo').show();
            } else {
                $('#menuEditInfo').hide();
            }
            if (pageContext.IsLastLevel) {
                editMemberId = parseInt($(target).data('memberid'), null);
                editUsername = $(target).data('username');
            } else {
                editAgentId = parseInt($(target).data('agentid'), null);
                editAgentLevel = parseInt($(target).data('agentlevel'), null);
            }
        },
        gridAfterBinding: function () {
            customerList.initInstruction();

            if (!pageContext.IsDirectDownline) {
                $(".cust-status").addClass('cust-status-none');
            }
        },
        resizeBox: function () {
            $("#boxsearch").width($("#grid").width());
            $("#breadcrumb").width($("#grid").width());
        },
        showAgent: function (agentId, agentName, agentLevel) {
            $('#newAgentId').val(agentId);
            $('#AgentLevel').val(agentLevel);
            $('#AgentName').val(agentName);
            $("#ajax-form").submit();
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage) {
            if (gridOptions && gridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage
                    : gridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
            }
            var params = {
                username: $.trim($("#Username").val()),
                status: $("#Status").val() !== -1 ? $.trim($("#Status").val()) : '',
                agentId: $("#AgentId").val(),
                agentLevel: $("#AgentLevel").val(),
                pageIndex: currentPage,
                pageSize: itemsPerPage
            };

            $.ajax({
                url: $('#search-form').attr('action'),
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                    } else {
                        if (pageContext.IsLastLevel) {
                            gridOptions.pagerOption = {
                                itemsPerPage: core.pageSizes[2],
                                currentPage: 0,
                                itemsPerPageArray: core.pageSizes,
                                showDetail: true,
                                afterPagedFunction: function () {
                                    customerList.gridAfterBinding();
                                }
                            };
                        } else {
                            gridOptions.pagerOption = {
                                itemsPerPage: data.ItemsPerPage,
                                itemsPerPageArray: core.pageSizes,
                                currentPage: data.CurrentPage,
                                totalItem: data.TotalItem,
                                pagerSelectCallBack: customerList.reRenderAfterPaged
                            };
                        }

                        gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                        $('#grid').show().fanexGrid(gridOptions);

                        customerList.gridAfterBinding();
                        customerList.resizeBox();
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        changeStatus: function (target, custId) {
            if (!pageContext.IsDirectDownline) {
                return;
            }
            var currentStatus, newStatus;
            currentStatus = $(target).find('.icon-status-open').length > 0 ? 1 : 0;
            newStatus = currentStatus === 0 ? 1 : 0;
            customerList.showAlert(target, custId, newStatus);
        },
        updateStatus: function (target, custId, statusId) {
            var params, url;
            if (pageContext.IsLastLevel) {
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

            url = pageContext.IsLastLevel ? "~/CustomerList/ChangeMemberStatus"
                : "~/CustomerList/ChangeAgentStatus";
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
                customerList.checkAll();
            } else {
                customerList.uncheckAll();
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
                if (pageContext.IsLastLevel) {
                    message = language.getString('ChangeStatusMember');
                } else {
                    message = language.getString('ChangeStatusAgent');
                }
            } else {
                if (pageContext.IsLastLevel) {
                    message = language.getString('ChangeCloseToOpenMember');
                } else {
                    message = language.getString('ChangeCloseToOpenAgent');
                }
            }
            var title = language.getString('ChangeStatus');
            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    popup.closeModal();
                    customerList.updateStatus(target, custId, statusId);
                }
            }, {
                label: language.getString('No'),
                action: function () {
                    popup.closeModal();
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
                url: core.resolveUrl('~/CustomerList/GetIpInformation'),
                type: 'post',
                data: { IpAddress: ipAddress },
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data !== null) {
                        popup.open({
                            title: msg.IpInformation,
                            message: data,
                            width: 420
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
                items: customerList.getEditItems(status),
                trigger: "left",
                ignoreRightClick: true,
                autoHide: true
            });
        },
        getEditItems: function (status) {
            var editItems = {}, items = [], index;
            if (pageContext.IsLastLevel) {
                items = [
                    {
                        name: language.getString("Sportsbook"),
                        callback: customerList.editSportsbook
                    },
                    {
                        name: language.getString("EditInfo"),
                        callback: customerList.editInfo
                    },
                    {
                        name: language.getString("ResetPassword"),
                        callback: customerList.resetPassword
                    },
                /*{ name: actions.NumberGame,
                callback: customerList.editSetting },
                { name: actions.Casino,
                callback: customerList.editSetting },
                { name: actions.VirtualSports,
                callback: customerList.editSetting },
                { name: actions.SBBetSetting,
                callback: customerList.editSetting },
                { name: actions.Commission,
                callback: customerList.editSetting },*/
                    {
                    name: language.getString("Password"),
                    callback: customerList.editSetting
                }
                ];
            } else {
                items = [
                    {
                        name: language.getString("Sportsbook"),
                        callback: customerList.editSportsbook
                    },
                    {
                        name: language.getString("EditInfo"),
                        callback: customerList.editInfo
                    },
                    {
                        name: language.getString("ResetPassword"),
                        callback: customerList.resetPassword
                    },
                /*{ name: actions.NumberGame,
                callback: customerList.editSetting },
                { name: actions.Casino,
                callback: customerList.editSetting },
                { name: actions.VirtualSports,
                callback: customerList.editSetting },
                { name: actions.AddDownline,
                callback: customerList.editSetting },
                { name: actions.Commission,
                callback: customerList.editSetting },*/
                    {
                    name: language.getString("Password"),
                    callback: customerList.editSetting
                }
                ];
            }

            if (pageContext.IsAdmin && !pageContext.IsLastLevel && status) {
                items.push({
                    name: language.getString("AddDownline"),
                    callback: customerList.addDownline
                });
            }
            if (pageContext.IsDirectDownline) {
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
            var url;
            if (pageContext.IsLastLevel) {
                url = "CustomerSetting/EditMember?memberId={0}"
                    .format(editMemberId);
            } else {
                url = "CustomerSetting/EditAgent?agentId={0}&agentLevel={1}"
                    .format(editAgentId, editAgentLevel);
            }
            popup.openFrame(url, {
                width: 940,
                height: 680
            });
        },
        editSetting: function () {
            alert('test');
        },
        editInfo: function () {
            var url;
            if (pageContext.IsLastLevel) {
                url = "CustomerList/EditInfoMember?memberId={0}&username={1}"
                    .format(editMemberId, editUsername);
            } else {
                url = "CustomerList/EditInfoAgent?agentId={0}"
                    .format(editAgentId);
            }
            popup.openFrame(url);
        },
        addDownline: function () {
            var url = "CustomerSetting/InsertCustomer?agentId={0}&agentLevel={1}"
                .format(editAgentId, editAgentLevel);
            popup.openFrame(url, {
                width: 940,
                height: 680
            });
        },
        resetPassword: function () {
            var url;
            if (pageContext.IsLastLevel) {
                url = "CustomerList/ResetPasswordMember?memberId={0}"
                    .format(editMemberId);
            } else {
                url = "CustomerList/ResetPasswordAgent?agentId={0}&agentLevel={1}"
                    .format(editAgentId, editAgentLevel);
            }
            popup.openFrame(url);
        }
    };
} (jQuery));

$(function () {
    'use strict';
    customerList.init();
});