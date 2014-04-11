/*global core*/

var underMaintenanceError = (function ($) {
    'use strict';
    core.DisableRecheckIn = true;
    return {
        init: function () {
            if ($(window.parent.document).find('iframe').length > 0) {
                window.parent.location.href = location.href;
                return;
            }

            setTimeout(function () { underMaintenanceError.reCheckUM(); }, 30000);
        },

        reCheckUM: function () {
            $.ajax({
                type: "POST",
                url: core.resolveUrl("~/Notify/UnderMaintenance"),
                dataType: "json",
                async: false,
                timeout: 120000
            })
            .done(function (result) {
                if (result.ErrCode !== 0) {
                    alert(result.ErrMsg);
                    setTimeout(function () { underMaintenanceError.reCheckUM(); }, 30000);
                    return;
                }

                if (result.Status) {
                    setTimeout(function () { underMaintenanceError.reCheckUM(); }, 30000);
                }
                else {
                    location.href = core.UrlRoot;
                }
            });
        }
    };
} (jQuery));

$(function () {
    'use strict';
    underMaintenanceError.init();
});