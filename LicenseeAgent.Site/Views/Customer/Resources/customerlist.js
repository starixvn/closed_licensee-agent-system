var customerList = function ($) {
    var isCalling = false
        , timerAutocomplete = null
        , preUserName
        , agentBreadCrumb = []
        , customerLink
        , customerType
        , combobox
        , lengthUsername = 2
        , statusObj = { All: -1, Close: 0, Open: 1 };

    var editAgentId = 0;
    var editAgentLevel = 0;
    var editMemberId = 0;

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
            $('#Username').keyup(function (event) {
                if (timerAutocomplete == null) {
                    timerAutocomplete = setInterval(customerList.autoComplete, 200);
                }
            });

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
            if (pageContext.IsLastLevel) {
                editMemberId = $(target).data('memberid');
            }
            else {
                editAgentId = $(target).data('agentid');
                editAgentLevel = $(target).data('agentlevel');
            }
            $('#menuSportsbook').hide();
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
                    gridOption.pagerOption = {
                        itemsPerPage: CORE.pageSizes[0],
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
                params = { memberid: custId, status: statusId };
            }
            else {
                params = { agentid: custId, status: statusId };
            }

            var url = pageContext.IsLastLevel ? "~/Customer/ChangeMemberStatus" : "~/Customer/ChangeAgentStatus";
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
        autoComplete: function () {
            var userName = $('#Username').val();
            customerList.clearTimer();
            if ($.trim(userName).length >= lengthUsername) {
                if (userName != preUserName) {
                    timerAutocomplete = setInterval(customerList.autoComplete, 200);
                }
                else {
                    customerList.getAutoComplete(userName);
                }
            }
            preUserName = userName;
        },
        clearTimer: function () {
            clearInterval(timerAutocomplete);
            timerAutocomplete = null;
        },
        getAutoComplete: function (userName) {
            if ($.trim(userName).length <= lengthUsername) {
                return;
            }
            if (!isCalling) {
                $.ajax({
                    url: CORE.resolveUrl('~/Customer/CustomerSearchAutoComplete'),
                    type: 'post',
                    data: {
                        text: userName,
                        agentid: $("#AgentId").val(),
                        agentLevel: $("#AgentLevel").val()
                    },
                    beforeSend: function () {
                        isCalling = true;
                    },
                    success: function (data) {
                        if (data.ErrCode == 0) {
                            $("#Username").autocomplete({
                                source: data.List
                            });
                        }
                    },
                    complete: function () {
                        isCalling = false;
                    }
                });
            }
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
                url: CORE.resolveUrl('~/Customer/GetIpInformation'),
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
                url = "SportsbookSetting/EditMember?memberId={0}".format(editMemberId);
            }
            else {
                url = "SportsbookSetting/EditAgent?agentId={0}&agentLevel={1}".format(editAgentId, editAgentLevel);
            }
            POPUP.openDialogFrame(url, { width: 960, height: 680 });
        },
        editSetting: function () {
            alert('test');
        },
        editInfo: function () {
            var url;
            if (pageContext.IsLastLevel) {
                url = "SportsbookSetting/EditMember?memberId={0}".format(editMemberId);
            }
            else {
                url = "SportsbookSetting/EditAgent?agentId={0}&agentLevel={1}".format(editAgentId, editAgentLevel);
            }
            POPUP.openDialogFrame(url, { width: 960, height: 680 });
        },
        addDownline: function () {
            var url;
            url = "SportsbookSetting/InsertCustomer?agentId={0}&agentLevel={1}".format(editAgentId, editAgentLevel);
            POPUP.openDialogFrame(url, { width: 960, height: 680 });
        },
        resetPassword: function () {
            alert("Reset Password");
            return;
            var url;
            if (pageContext.IsLastLevel) {
                url = "SportsbookSetting/EditMember?memberId={0}".format(editMemberId);
            }
            else {
                url = "SportsbookSetting/EditAgent?agentId={0}&agentLevel={1}".format(editAgentId, editAgentLevel);
            }
            POPUP.openDialogFrame(url, { width: 960, height: 680 });
        }
    }
} (jQuery);
$(function () {
    customerList.init();
});