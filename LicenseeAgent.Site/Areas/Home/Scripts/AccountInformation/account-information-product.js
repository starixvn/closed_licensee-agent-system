/*global CORE, LANGUAGE, POPUP, pageContext */
'use strict';

var HOME = HOME || {};
HOME.ACCOUNT_INFORMATION = HOME.ACCOUNT_INFORMATION || {};
HOME.ACCOUNT_INFORMATION.PRODUCT = (function ($this, $) {
    $this.init = function () {
        $('#panel-products .product[title]').tooltip();
        $('#panel-products input:enabled').click(function (e) {
            HOME.ACCOUNT_INFORMATION.PRODUCT.updateStatus(this);
            e.preventDefault();
        });
    };

    $this.updateStatus = function (input) {
        var disable = !$(input).is(':checked');
        var productId = $(input).data('product-id');
        var productName = $.trim($(input).parent().text());
        var custId = pageContext.CustId;
        if (pageContext.IsMember) {
            updateMemberStatus(input, custId, disable, productId, productName);
        } else {
            updateAgentStatus(input, custId, disable, productId, productName);
        }
    };

    var updateAgentStatus = function (input, agentId, disable, productId, productName) {
        var message = disable
            ? LANGUAGE.getString('AlertDisableAgentProduct').format(productName)
            : LANGUAGE.getString('AlertEnableAgentProduct').format(productName);
        var title = LANGUAGE.getString('ChangeStatus');
        var ajaxUpdateStatus = function (input, disable, productId, isAllDownLine) {
            CORE.ajaxJSON({
                url: CORE.resolveUrl('~/ProductSetting/Status/UpdateAgent'),
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
            label: LANGUAGE.getString('Yes'),
            action: function () {
                POPUP.close();
                ajaxUpdateStatus(input, disable, productId, true);
            }
        }, {
            label: LANGUAGE.getString('No'),
            action: function () {
                POPUP.close();
                if (!disable) {
                    ajaxUpdateStatus(input, disable, productId, false);
                }
            }
        }];

        POPUP.confirm({
            title: title,
            message: message,
            buttons: buttons
        });
    };

    var updateMemberStatus = function (input, custId, disable, productId, productName) {
        var ajaxUpdateStatus = function (input, disable, productId) {
            CORE.ajaxJSON({
                url: CORE.resolveUrl('~/ProductSetting/Status/UpdateMember'),
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
            label: LANGUAGE.getString('Yes'),
            action: function () {
                POPUP.close();
                ajaxUpdateStatus(input, disable, productId);
            }
        }, {
            label: LANGUAGE.getString('No'),
            action: function () {
                POPUP.close();
            }
        }];

        POPUP.confirm({
            title: LANGUAGE.getString('ChangeStatus'),
            message: LANGUAGE.getString('AlertDisableMemberProduct').format(productName),
            buttons: buttons
        });
    };

    return $this;
} (HOME.ACCOUNT_INFORMATION.PRODUCT || {}, jQuery));