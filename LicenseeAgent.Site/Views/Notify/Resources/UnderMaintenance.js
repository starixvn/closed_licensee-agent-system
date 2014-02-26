var UNDERMAINTENANCE = function ($) {
    return {
        init: function () {
            if ($(window.parent.document).find('iframe').length > 0) {
                window.parent.location.href = location.href;
                return;
            }

            setTimeout(function () { UNDERMAINTENANCE.reCheckUM(); }, 30000);
        },

        reCheckUM: function () {
            $.ajax({
                type: "GET",
                url: CORE.resolveUrl("~/NotifyController/UnderMaintenance"),
                dataType: "json",
                async: false,
                timeout: 120000
            })
            .done(function (result) {
                if (result.ErrCode != 0) {
                    location.href = CORE.UrlSignOut;
                }
                else {
                    setTimeout(function () { UNDERMAINTENANCE.reCheckUM(); }, 30000);
                }
            });
        }
    };
} (jQuery);
$(function () {
    UNDERMAINTENANCE.init();
});