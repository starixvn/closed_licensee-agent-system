/*global CORE, LANGUAGE, POPUP, ENUMS, pageContext */
'use strict';

var HOME = HOME || {};
HOME.ACCOUNT_INFORMATION = HOME.ACCOUNT_INFORMATION || {};
HOME.ACCOUNT_INFORMATION.INFO = (function ($this, $) {
    $this.init = function () {
        $("#customer-status").click(function () {
            HOME.ACCOUNT_INFORMATION.INFO.changeStatus(this);
        });

        $(".cust-info .icon-collapse").click(function () {
            if ($(this).hasClass("icon-collapse")) {
                $(this).parents(".cust-info").find("table").hide();
                $(this).removeClass("icon-collapse").addClass("icon-expand");
            }
            else {
                $(this).parents(".cust-info").find("table").show();
                $(this).removeClass("icon-expand").addClass("icon-collapse");
            }
        });
    };

    $this.changeStatus = function (target) {
        if (!(pageContext.IsDirectDownLine || pageContext.IsAdmin)) {
            return;
        }

        var currentStatus, newStatus;
        var agentId = parseInt($("#AgentId").val());
        if (!pageContext.IsAdmin && pageContext.AgentId === agentId) {
            return;
        }

        currentStatus = $(target).hasClass('icon-status-open')
            ? ENUMS.AccountStatus.Open
            : ENUMS.AccountStatus.Closed;
        newStatus = currentStatus === ENUMS.AccountStatus.Closed
            ? ENUMS.AccountStatus.Open
            : ENUMS.AccountStatus.Closed;
        alertChangeStatus(target, agentId, newStatus);
    };

    var alertChangeStatus = function (target, custId, statusId) {
        var message = '';
        if (statusId === ENUMS.AccountStatus.Closed) {
            if (pageContext.IsMember) {
                message = LANGUAGE.getString('ChangeStatusMember');
            } else {
                message = LANGUAGE.getString('ChangeStatusAgent');
            }
        } else {
            if (pageContext.IsMember) {
                message = LANGUAGE.getString('ChangeCloseToOpenMember');
            } else {
                message = LANGUAGE.getString('ChangeCloseToOpenAgent');
            }
        }
        var title = LANGUAGE.getString('ChangeStatus');
        var buttons = [{
            label: LANGUAGE.getString('Yes'),
            action: function () {
                POPUP.close();
                HOME.ACCOUNT_INFORMATION.INFO.updateStatus(target, custId, statusId);
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
        var params, url;
        if (pageContext.IsMember) {
            url = CORE.resolveUrl('~/CustomerList/Member/ChangeStatus');
            params = {
                memberid: custId,
                status: statusId
            };
        }
        else {
            url = CORE.resolveUrl('~/CustomerList/Agent/ChangeStatus');
            params = {
                agentid: custId,
                status: statusId
            };
        }
        CORE.ajaxJSON({
            url: url,
            type: 'post',
            data: params
        }, function () {
            if (statusId === ENUMS.AccountStatus.Closed) {
                $(target).addClass('icon-status-closed')
                        .removeClass('icon-status-open')
                        .find('span')
                        .text(LANGUAGE.getString('Closed'));
                $('.cust-info').children(':first')
                        .addClass('closed')
                        .removeClass('open');
            } else {
                $(target).addClass('icon-status-open')
                        .removeClass('icon-status-closed')
                        .find('span')
                        .text(LANGUAGE.getString('Open'));
                $('.cust-info').children(':first')
                        .addClass('open')
                        .removeClass('closed');
            }
        });
    };

    return $this;
} (HOME.ACCOUNT_INFORMATION.INFO || {}, jQuery));