/*global CORE*/
    CORE.DisableRecheckIn = true;
var underMaintenance = (function ($) {
    /*jshint strict:false */
    return {
        init: function () {
            if ($(window.parent.document).find('iframe').length > 0) {
                window.parent.location.href = location.href;
                return;
            }

            setTimeout(function () { underMaintenance.reCheckUM(); }, 30000);
        },

        reCheckUM: function () {
            $.ajax({
                type: "POST",
                url: CORE.resolveUrl("~/Notify/UnderMaintenance"),
                dataType: "json",
                async: false,
                timeout: 120000
            })
            .done(function (result) {
                if (result.ErrCode !== 0) {
                    alert(result.ErrMsg);
                    setTimeout(function () { underMaintenance.reCheckUM(); }, 30000);
                    return;
                }

                if (result.Status) {
                    setTimeout(function () { underMaintenance.reCheckUM(); }, 30000);
                }
                else {
                    location.href = CORE.UrlRoot;
                }
            });
        }
    };
} (jQuery));

$(function () {
    /*jshint strict:false */
    underMaintenance.init();
});