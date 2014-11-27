/*global core, language, popup, pageContext */
'use strict';

var home = home || {};
home.accountInformation = home.accountInformation || {};
home.accountInformation.info = (function ($this, $) {
    $this.init = function () {
        $("#customer-status").click(function () {
            home.accountInformation.info.changeStatus(this);
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
            ? core.enums.AccountStatus.Open
            : core.enums.AccountStatus.Closed;
        newStatus = currentStatus === core.enums.AccountStatus.Closed
            ? core.enums.AccountStatus.Open
            : core.enums.AccountStatus.Closed;
        home.accountInformation.info.alertChangeStatus(target, agentId, newStatus);
    };

    $this.alertChangeStatus = function (target, custId, statusId) {
        var message = '';
        if (statusId === core.enums.AccountStatus.Closed) {
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
                home.accountInformation.info.updateStatus(target, custId, statusId);
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
    };

    $this.updateStatus = function (target, custId, statusId) {
        var params, url;
        if (pageContext.IsMember) {
            url = core.resolveUrl('~/CustomerList/Member/ChangeStatus');
            params = {
                memberid: custId,
                status: statusId
            };
        }
        else {
            url = core.resolveUrl('~/CustomerList/Agent/ChangeStatus');
            params = {
                agentid: custId,
                status: statusId
            };
        }
        core.ajaxJSON({
            url: url,
            type: 'post',
            data: params
        }, function () {
            if (statusId === core.enums.AccountStatus.Closed) {
                $(target).addClass('icon-status-closed')
                        .removeClass('icon-status-open')
                        .find('span')
                        .text(language.getString('Closed'));
                $('.cust-info').children(':first')
                        .addClass('closed')
                        .removeClass('open');
            } else {
                $(target).addClass('icon-status-open')
                        .removeClass('icon-status-closed')
                        .find('span')
                        .text(language.getString('Open'));
                $('.cust-info').children(':first')
                        .addClass('open')
                        .removeClass('closed');
            }
        });
    };

    return $this;
} (home.accountInformation.info || {}, jQuery));