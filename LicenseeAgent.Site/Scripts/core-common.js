/*global language, popup*/
var core = core || {};
core.common = (function ($) {
    'use strict';
    return {
        showIP: function (ipAddress) {
            $.ajax({
                url: core.resolveUrl('~/Common/IP'),
                type: 'post',
                data: {
                    IpAddress: ipAddress
                },
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    popup.open({
                        title: language.getString("IpInformation"),
                        message: data
                    });
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        }
    };
} (jQuery));