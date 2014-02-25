var UNDERMAINTENANCE = function ($) {
    return {
        init: function () {
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