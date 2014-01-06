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

    var urlGetCustomerList = $('#search-form').attr('action');
    var urlCustomerSearchAutoComplete = $("#Username").attr('urlautosearch');
    var urlChangeStatus = $("#grid").data('url-change-status');
    var urlIpInformation = $("#grid").data('url-ip');
    var isLastLevel = $("#grid").data('is-last-level');
    var editAgentId = 0;
    var editAgentLevel = 0;
    var editMemberId = 0;

    return {
        init: function () {
            $('.selectpicker').selectpicker();

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
        initSectionInfo: function () {
            $('.infos').each(function () {
                $(this).click(function () {
                    customerList.getIpInformation($(this).html());
                });
            });
        },
        gridAfterBinding: function () {
            customerList.initSectionInfo();

            $('.make-switch')['bootstrapSwitch']();
            $('.cust-status').on('switch-change', function (e) {
                var status = statusObj.All;
                if ($(this).find('div:first').hasClass("switch-off")) {
                    $(this).find('div:first').removeClass("switch-off");
                    $(this).find('div:first').addClass("switch-on");
                    status = statusObj.Close;
                }
                else {
                    $(this).find('div:first').removeClass("switch-on");
                    $(this).find('div:first').addClass("switch-off");
                    status = statusObj.Open;
                }
                if (!pageContext.IsDirectDownline) {
                    return;
                }
                customerList.showAlert(true, $(this), $(this).data('id'), status);
            });

            if (!pageContext.IsDirectDownline) {
                $(".cust-status").addClass('cust-status-none');
            }
        },
        resizeBox: function () {
            $("#boxsearch").width($("#grid").width());
            $("#breadcrumb").width($("#grid").width());
            $(".legend").width($("#grid").width());
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
                url: urlGetCustomerList,
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
        changeStatus: function (target, custId, statusId, url) {
            var params;
            if (pageContext.IsLastLevel) {
                params = { memberid: custId, status: statusId };
            }
            else {
                params = { agentid: custId, status: statusId };
            }

            $.ajax({
                url: urlChangeStatus,
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode != 0)
                        alert(data.ErrMsg);
                    else {
                        if (statusId == 0) {
                            target.find('div:first').removeClass("switch-on");
                            target.find('div:first').addClass("switch-off");
                        }
                        else {
                            target.find('div:first').removeClass("switch-off");
                            target.find('div:first').addClass("switch-on");
                        }
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        getStatus: function (statusId) {
            if (statusId == 0) {
                return '';
            }
            return 'checked';
        },
        onShowTop: function () {
            $.contextMenu({
                selector: '.icon-arrow',
                items: showItems,
                trigger: "left",
                ignoreRightClick: true,
                autoHide: true
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
                    url: urlCustomerSearchAutoComplete,
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
                    customerList.changeStatus(target, custId, statusId);
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
            $.ajax({ url: urlIpInformation,
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
        editAgent: function (agentId, agentLevel) {
            editAgentId = agentId;
            editAgentLevel = agentLevel;
            $.contextMenu({
                selector: '.cell-' + agentId,
                items: customerList.getEditItems(),
                trigger: "left",
                ignoreRightClick: true,
                autoHide: true
            });
        },
        editMember: function (memberId) {
            editMemberId = memberId;
            $.contextMenu({
                selector: '.cell-' + memberId,
                items: customerList.getEditItems(),
                trigger: "left",
                ignoreRightClick: true,
                autoHide: true
            });
        },
        getEditItems: function () {
            var editItems = {};
            var items = [];
            if (pageContext.IsLastLevel) {
                items = [{ name: actions.Sportsbook, callback: customerList.editSportsbook },
                /*{ name: actions.NumberGame, callback: customerList.editSetting },
                { name: actions.Casino, callback: customerList.editSetting },
                { name: actions.VirtualSports, callback: customerList.editSetting },
                { name: actions.EditInfo, callback: customerList.editSetting },
                { name: actions.SBBetSetting, callback: customerList.editSetting },
                { name: actions.Commission, callback: customerList.editSetting },*/
                        {name: actions.Password, callback: customerList.editSetting}];
            }
            else {
                items = [{ name: actions.Sportsbook, callback: customerList.editSportsbook },
                /*{ name: actions.NumberGame, callback: customerList.editSetting },
                { name: actions.Casino, callback: customerList.editSetting },
                { name: actions.VirtualSports, callback: customerList.editSetting },
                { name: actions.EditInfo, callback: customerList.editSetting },
                { name: actions.AddDownline, callback: customerList.editSetting },
                { name: actions.Commission, callback: customerList.editSetting },*/
                        {name: actions.Password, callback: customerList.editSetting}];
            }

            if (pageContext.IsDirectDownline) {
                for (var index = 0; index < items.length; index++) {
                    editItems[index] = { name: "<span class='action-content'><i class='icon-canedit'></i>" + items[index].name + "</span>", callback: items[index].callback };
                }
            }
            else {
                for (var index = 0; index < items.length; index++) {
                    editItems[index] = { name: "<span class='action-content'><i class='icon-canview'></i>" + items[index].name + "</span>", callback: items[index].callback };
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
        }
    }
} (jQuery);
$(function () {
    customerList.init();
});