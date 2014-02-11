var customerList = function ($) {
    var statusObj = { All: -1, Close: 0, Open: 1 };
    var editAgentId = 0;
    var editAgentLevel = 0;
    var editMemberId = 0;
    var editUsername = "";

    return {
        init: function () {
            $('.selectpicker').selectpicker();

            // create action context menu
            context.init({
                preventDoubleContext: false,
                beforeShow: customerList.updateActionMenus
            });
            context.attach('.icon-customer-edit', [
                { id: 'menuSportsbook', icon: 'icon-account-logout', text: LANGUAGE.getString('Sportsbook'), href: '#', action: customerList.editSportsbook },
                { id: 'menuEditInfo', icon: 'icon-account-logout', text: LANGUAGE.getString('EditInfo'), href: '#', action: customerList.editInfo },
                { id: 'menuResetPassword', icon: 'icon-account-logout', text: LANGUAGE.getString('ResetPassword'), href: '#', action: customerList.resetPassword },
                { id: 'menuAddDownline', icon: 'icon-account-logout', text: LANGUAGE.getString('AddDownline'), href: '#', action: customerList.addDownline }
            ]);

            // create select context menu
            context.init({
                preventDoubleContext: false
            });
            context.attach('#select-dropdown', [
                { text: LANGUAGE.getString('All'), href: '#', action: customerList.checkAll },
                { text: LANGUAGE.getString('None'), href: '#', action: customerList.uncheckAll }
            ]);

            // breadcrum
            $('.breadcrumb a').click(function () {
                var agentId = $(this).data('agent-id');
                var agentLevel = $(this).data('agent-level');
                var agentName = $(this).text();
                customerList.showAgent(agentId, agentName, agentLevel);
            });

            // search
            $('#search-form').submit(function (e) {
                customerList.reRenderAfterPaged();
                e.preventDefault();
            });

            // username suggestion
            $('#Username').typeahead({ source: customerList.getSuggestion, minLength: 2 });

            $(".combobox-container input[type=text]").attr('readonly', 'readonly');

            // get customer list
            customerList.reRenderAfterPaged();
        },
        initInstruction: function () {
            $('.instruction-container').each(function () {
                $(this).click(function () {
                    customerList.getIpInformation($(this).html());
                });
            });
        },
        updateActionMenus: function (target) {
            if (!pageContext.IsAdmin || $(target).attr('data-status') == 0 || pageContext.IsLastLevel) {
                $('#menuAddDownline').hide();
            }
            else {
                $('#menuAddDownline').show();
            }

            if (pageContext.IsAdmin || pageContext.IsDirectDownline) {
                $('#menuEditInfo').show();
            }
            else {
                $('#menuEditInfo').hide();
            }

            if (pageContext.IsLastLevel) {
                editMemberId = parseInt($(target).data('memberid'));
                editUsername = $(target).data('username');
            }
            else {
                editAgentId = parseInt($(target).data('agentid'));
                editAgentLevel = parseInt($(target).data('agentlevel'));
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
        reRenderAfterPaged: function () {
            var params = {
                username: $.trim($("#Username").val()),
                status: $("#Status").val() != -1 ? $.trim($("#Status").val()) : '',
                agentId: $("#AgentId").val(),
                agentLevel: $("#AgentLevel").val()
            };

            $.ajax({
                url: $('#search-form').attr('action'),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode != 0) {
                        alert(data.ErrMsg);
                    }
                    else {
                        gridOption.pagerOption = {
                            itemsPerPage: CORE.pageSizes[2],
                            currentPage: 0,
                            itemsPerPageArray: CORE.pageSizes,
                            showDetail: true,
                            afterPagedFunction: function () {
                                customerList.gridAfterBinding();
                            }
                        };

                        gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                        $('#grid').show().fanexGrid(gridOption);

                        customerList.gridAfterBinding();
                        customerList.resizeBox();
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        changeStatus: function (target, custId) {
            if (!pageContext.IsDirectDownline) {
                return;
            }
            var currentStatus = $(target).find('.icon-status-open').length > 0 ? 1 : 0;
            var newStatus = currentStatus == 0 ? 1 : 0;
            customerList.showAlert(true, target, custId, newStatus);
        },
        updateStatus: function (target, custId, statusId, url) {
            var params;
            if (pageContext.IsLastLevel) {
                params = { memberid: custId, status: statusId, agentid: $("#AgentId").val() };
            }
            else {
                params = { agentid: custId, status: statusId };
            }

            var url = pageContext.IsLastLevel ? "~/CustomerList/ChangeMemberStatus" : "~/CustomerList/ChangeAgentStatus";
            $.ajax({
                url: CORE.resolveUrl(url),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode != 0) {
                        alert(data.ErrMsg);
                        return;
                    }
                    if (statusId == 0) {
                        $(target).find('.icon-status-open').addClass('icon-status-closed').removeClass('icon-status-open').find('span').text('Closed');
                    }
                    else {
                        $(target).find('.icon-status-closed').addClass('icon-status-open').removeClass('icon-status-closed').find('span').text('Open');
                    }
                    $('#' + custId).attr('data-status', statusId);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        checkAllItem: function (itemAll) {
            if ($(itemAll).hasClass("icon-uncheck-all")) {
                customerList.checkAll();
            }
            else {
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
                if ($(".cell-select").length == $(".icon-checked").length) {
                    $(".icon-uncheck-all").attr("class", "icon-checked-all");
                }
            }
            else {
                $(itemSelect).removeClass("icon-checked");
                $(itemSelect).addClass("icon-uncheck");
                $(".icon-checked-all").attr("class", "icon-uncheck-all");
            }
        },
        getSuggestion: function (query, process) {
            return $.ajax({
                url: CORE.resolveUrl('~/CustomerList/CustomerSearchAutoComplete'),
                type: 'post',
                data: {
                    userName: query,
                    agentId: $("#AgentId").val(),
                    agentLevel: $("#AgentLevel").val()
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        return process(data.List);
                    }
                }
            });
        },
        showAlert: function (isConfirm, target, custId, statusId) {
            var buttons = new Array();
            buttons.push({
                text: 'YES',
                click: function () {
                    customerList.closeDialog();
                    customerList.updateStatus(target, custId, statusId);
                }
            });
            buttons.push({
                text: 'NO',
                click: function () {
                    customerList.closeDialog();
                }
            });
            if (statusId == statusObj.Close) {
                if (pageContext.IsLastLevel) {
                    $('#dialog-confirm').find('p').html(msg.OpenToCloseMember);
                }
                else {
                    $('#dialog-confirm').find('p').html(msg.OpenToCloseAgent);
                }
            }
            else {
                if (pageContext.IsLastLevel) {
                    $('#dialog-confirm').find('p').html(msg.CloseToOpenMember);
                }
                else {
                    $('#dialog-confirm').find('p').html(msg.CloseToOpenAgent);
                }
            }
            $("#dialog-confirm").dialog({
                resizable: false,
                width: 400,
                modal: true,
                buttons: buttons,
                close: function (event, ui) {
                }
            });
        },
        closeDialog: function () {
            $('#dialog-confirm').dialog('close');
        },
        getIpInformation: function (ipAddress) {
            $.ajax({
                url: CORE.resolveUrl('~/CustomerList/GetIpInformation'),
                type: 'post',
                data: { IpAddress: ipAddress },
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data != null) {
                        $('#instruction').html(data);
                        var dialog = POPUP.openDialog($('#instruction'), msg.IpInformation, { modal: false, clickout: true, openid: $(this).attr('id') });
                        $('.ui-tooltip').remove();
                    }
                },
                complete: function () {
                    CORE.unblockUI();
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
            var editItems = {};
            var items = [];
            if (pageContext.IsLastLevel) {
                items = [
                    { name: LANGUAGE.getString("Sportsbook"), callback: customerList.editSportsbook },
                    { name: LANGUAGE.getString("EditInfo"), callback: customerList.editInfo },
                    { name: LANGUAGE.getString("ResetPassword"), callback: customerList.resetPassword },
                /*{ name: actions.NumberGame, callback: customerList.editSetting },
                { name: actions.Casino, callback: customerList.editSetting },
                { name: actions.VirtualSports, callback: customerList.editSetting },
                { name: actions.SBBetSetting, callback: customerList.editSetting },
                { name: actions.Commission, callback: customerList.editSetting },*/
                    {name: LANGUAGE.getString("Password"), callback: customerList.editSetting }
                ];
            }
            else {
                items = [
                    { name: LANGUAGE.getString("Sportsbook"), callback: customerList.editSportsbook },
                    { name: LANGUAGE.getString("EditInfo"), callback: customerList.editInfo },
                    { name: LANGUAGE.getString("ResetPassword"), callback: customerList.resetPassword },
                /*{ name: actions.NumberGame, callback: customerList.editSetting },
                { name: actions.Casino, callback: customerList.editSetting },
                { name: actions.VirtualSports, callback: customerList.editSetting },
                { name: actions.AddDownline, callback: customerList.editSetting },
                { name: actions.Commission, callback: customerList.editSetting },*/
                    {name: LANGUAGE.getString("Password"), callback: customerList.editSetting }
                ];
            }

            if (pageContext.IsAdmin && !pageContext.IsLastLevel && status) {
                items.push({ name: LANGUAGE.getString("AddDownline"), callback: customerList.addDownline });
            }

            if (pageContext.IsDirectDownline) {
                for (var index = 0; index < items.length; index++) {
                    editItems[index] = { name: "<span class='action-content'><i class='icon-customer-canedit'></i>" + items[index].name + "</span>", callback: items[index].callback };
                }
            }
            else {
                for (var index = 0; index < items.length; index++) {
                    editItems[index] = { name: "<span class='action-content'><i class='icon-customer-canview'></i>" + items[index].name + "</span>", callback: items[index].callback };
                }
            }

            return editItems;
        },
        editSportsbook: function () {
            var url;
            if (pageContext.IsLastLevel) {
                url = "CustomerSetting/EditMember?memberId={0}".format(editMemberId);
            }
            else {
                url = "CustomerSetting/EditAgent?agentId={0}&agentLevel={1}".format(editAgentId, editAgentLevel);
            }
            POPUP.openDialogFrame(url, { width: 960, height: 680 });
        },
        editSetting: function () {
            alert('test');
        },
        editInfo: function () {
            var url;
            if (pageContext.IsLastLevel) {
                url = "CustomerList/EditInfoMember?memberId={0}&username={1}".format(editMemberId, editUsername);
            }
            else {
                url = "CustomerList/EditInfoAgent?agentId={0}".format(editAgentId);
            }
            POPUP.openDialogFrame(url, { width: 420, height: 400 });
        },
        addDownline: function () {
            var url;
            url = "CustomerSetting/InsertCustomer?agentId={0}&agentLevel={1}".format(editAgentId, editAgentLevel);
            POPUP.openDialogFrame(url, { width: 960, height: 680 });
        },
        resetPassword: function () {
            var url;
            if (pageContext.IsLastLevel) {
                url = "CustomerList/ResetPasswordMember?memberId={0}".format(editMemberId)
            }
            else {
                url = "CustomerList/ResetPasswordAgent?agentId={0}&agentLevel={1}".format(editAgentId, editAgentLevel);
            }
            POPUP.openDialogFrame(url, { width: 420, height: 400 });
        }
    }
} (jQuery);
$(function () {
    customerList.init();
});