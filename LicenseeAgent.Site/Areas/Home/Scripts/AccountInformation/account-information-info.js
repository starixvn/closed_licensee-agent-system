/*global core, language, popup, pageContext */

var home = home || {};
home.accountInformation = home.accountInformation || {};
home.accountInformation.info = (function ($) {
    'use strict';
    var statusObj = { Close: 0, Open: 1 };
    var updateAgentStatusUrl = '~/CustomerList/Agent/ChangeStatus';
    var updateMemberStatusUrl = '~/CustomerList/Member/ChangeStatus';
    return {
        init: function () {
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
        },
        changeStatus: function (target) {
            if (!(pageContext.IsDirectDownLine || pageContext.IsAdmin)) {
                return;
            }

            var currentStatus, newStatus;
            var agentId = parseInt($("#AgentId").val());
            if (!pageContext.IsAdmin && pageContext.AgentId === agentId) {
                return;
            }

            currentStatus = $(target).hasClass('icon-status-open') ? 1 : 0;
            newStatus = currentStatus === 0 ? 1 : 0;
            home.accountInformation.info.alertChangeStatus(target, agentId, newStatus);
        },
        alertChangeStatus: function (target, custId, statusId) {
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
        },
        updateStatus: function (target, custId, statusId) {
            var params, url;
            if (pageContext.IsMember) {
                url = updateMemberStatusUrl;
                params = {
                    memberid: custId,
                    status: statusId
                };
            }
            else {
                url = updateAgentStatusUrl;
                params = {
                    agentid: custId,
                    status: statusId
                };
            }
            core.ajaxJSON({
                url: core.resolveUrl(url),
                type: 'post',
                data: params
            }, function () {
                if (statusId === core.enums.Status.Closed) {
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
        }
    };
} (jQuery));