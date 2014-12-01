/*global CORE, LANGUAGE, POPUP*/
/*exported PAGE*/
'use strict';

var PAGE = (function () {
    return {
        showIP: function (ipAddress) {
            CORE.ajaxHTML({
                url: CORE.resolveUrl('~/Common/IP'),
                data: {
                    IpAddress: ipAddress
                }
            }, function (response) {
                POPUP.open({
                    title: LANGUAGE.getString("IpInformation"),
                    message: response
                });
            });
        }
    };
} ());