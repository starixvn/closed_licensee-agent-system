/*global core, language, popup, pageContext */

var home = home || {};
home.accountInformation = home.accountInformation || {};
home.accountInformation.product = (function ($) {
    'use strict';
    var updateAgentProductStatusUrl = '~/ProductSetting/Status/UpdateAgent';
    var updateMemberProductStatusUrl = '~/ProductSetting/Status/UpdateMember';
    return {
        init: function () {
            $('#panel-products .product[title]').tooltip();
            $('#panel-products input:enabled').click(function (e) {
                home.accountInformation.product.updateStatus(this);
                e.preventDefault();
            });
        },
        updateStatus: function (input) {
            var disable = !$(input).is(':checked');
            var productId = $(input).data('product-id');
            var productName = $.trim($(input).parent().text());
            var custId = pageContext.CustId;
            if (pageContext.IsMember) {
                home.accountInformation.product.updateMemberStatus(input, custId, disable, productId, productName);
            } else {
                home.accountInformation.product.updateAgentStatus(input, custId, disable, productId, productName);
            }
        },
        updateAgentStatus: function (input, agentId, disable, productId, productName) {
            var message = disable
                ? language.getString('AlertDisableAgentProduct').format(productName)
                : language.getString('AlertEnableAgentProduct').format(productName);
            var title = language.getString('ChangeStatus');
            var ajaxUpdateStatus = function (input, disable, productId, isAllDownLine) {
                core.ajaxJSON({
                    url: core.resolveUrl(updateAgentProductStatusUrl),
                    data: {
                        agentId: agentId,
                        productId: productId,
                        disable: disable,
                        isAllDownLine: isAllDownLine
                    }
                }, function () {
                    $(input).prop('checked', !disable);
                });
            };

            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    popup.close();
                    ajaxUpdateStatus(input, disable, productId, true);
                }
            }, {
                label: language.getString('No'),
                action: function () {
                    popup.close();
                    if (!disable) {
                        ajaxUpdateStatus(input, disable, productId, false);
                    }
                }
            }];

            popup.confirm({
                title: title,
                message: message,
                buttons: buttons
            });
        },
        updateMemberStatus: function (input, custId, disable, productId, productName) {
            var ajaxUpdateStatus = function (input, disable, productId) {
                core.ajaxJSON({
                    url: core.resolveUrl(updateMemberProductStatusUrl),
                    data: {
                        custId: custId,
                        productId: productId,
                        disable: disable
                    }
                }, function () {
                    $(input).prop('checked', !disable);
                });
            };

            if (!disable) {
                ajaxUpdateStatus(input, disable, productId);
                return;
            }

            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    popup.close();
                    ajaxUpdateStatus(input, disable, productId);
                }
            }, {
                label: language.getString('No'),
                action: function () {
                    popup.close();
                }
            }];

            popup.confirm({
                title: language.getString('ChangeStatus'),
                message: language.getString('AlertDisableMemberProduct').format(productName),
                buttons: buttons
            });
        }
    };
} (jQuery));